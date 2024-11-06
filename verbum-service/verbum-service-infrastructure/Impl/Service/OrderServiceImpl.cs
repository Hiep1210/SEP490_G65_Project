using AutoMapper;
using Lombok.NET;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using verbum_service_application.Service;
using verbum_service_domain.Common;
using verbum_service_domain.Common.ErrorModel;
using verbum_service_domain.DTO.Request;
using verbum_service_domain.DTO.Response;
using verbum_service_domain.Models;
using verbum_service_domain.Utils;
using verbum_service_infrastructure.DataContext;

namespace verbum_service_infrastructure.Impl.Service
{
    [RequiredArgsConstructor]
    public partial class OrderServiceImpl : OrderService
    {
        private readonly verbumContext context;
        private readonly IMapper mapper;
        private readonly CurrentUser currentUser;

        public async Task CreateOrder(Order info)
        {
            try
            {
                info.ClientId = currentUser.Id;
                context.Orders.Add(info);
                await context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task AddRangeMiddle(Guid orderId, List<string> languageIds)
        {
            try
            {
                var categories = context.Languages.Where(c => languageIds.Contains(c.LanguageId)).ToList();

                var order = context.Orders.Find(orderId);
                if (order != null)
                {
                    foreach (var category in categories)
                    {
                        order.TargetLanguages.Add(category);
                    }
                }

                await context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw;
            }
        }


        public async Task<List<OrderDetailsResponse>> GetAllOrder()
        {
            List<Order> orders = new List<Order>();
            Guid clientId = currentUser.Id;
            switch (currentUser.Role)
            {
                case UserRole.CLIENT:
                    orders = await context.Orders.Include(o => o.TargetLanguages).Include(o => o.OrderReferences)
                        .Where(x => x.ClientId == clientId)
                        .ToListAsync();
                    break;
                case UserRole.ADMIN: 
                case UserRole.STAFF: 
                case UserRole.DIRECTOR:
                case UserRole.LINGUIST:
                case UserRole.MANAGER: 
                    orders = await context.Orders.Include(o => o.TargetLanguages).Include(o => o.OrderReferences)
                        .ToListAsync();
                    break;
                default:
                    throw new BusinessException(AlertMessage.Alert(ValidationAlertCode.NOT_FOUND, "Role"));
            }
            List<OrderDetailsResponse> list = mapper.Map<List<OrderDetailsResponse>>(orders);
            return list;
        }

        public async Task<OrderDetailsResponse> GetOrderDetails(Guid id)
        {
            Order orders = new Order();
            orders = await context.Orders.FirstOrDefaultAsync(x => x.OrderId == id);
            if (ObjectUtils.IsEmpty(orders))
            {
                throw new BusinessException(AlertMessage.Alert(ValidationAlertCode.NOT_FOUND, "Order"));
            }

            await context.Entry(orders)
                    .Collection(o => o.TargetLanguages)
                    .LoadAsync();

            await context.Entry(orders)
                    .Collection(o => o.OrderReferences)
                    .LoadAsync();

            OrderDetailsResponse orderResponse = mapper.Map<OrderDetailsResponse>(orders);
            return orderResponse;
        }

        public async Task UpdateOrder(OrderUpdate request)
        {
            int records = await context.Orders
                .Where(x => x.OrderId == request.OrderId)
                .ExecuteUpdateAsync(x => x.SetProperty(u => u.OrderName, request.OrderName)
                                        .SetProperty(u => u.OrderStatus, OrderStatus.NEW.ToString())
                                        .SetProperty(u => u.OrderNote, request.OrderNote)
                                        .SetProperty(u => u.SourceLanguageId, request.SourceLanguageId)
                                        .SetProperty(u => u.DueDate, request.DueDate)
                                        .SetProperty(u => u.HasTranslateService, request.TranslateService)
                                        .SetProperty(u => u.HasEditService, request.EditService)
                                        .SetProperty(u => u.HasEvaluateService, request.EvaluateService));
            if (records < 1) throw new BusinessException(ValidationAlertCode.UPDATE_RECORD_FAIL);
        }

        public async Task UpdateOrderPrice(Guid orderId, decimal price)
        {
            Order order = context.Orders.Include(o => o.Discount).FirstOrDefault(x => x.OrderId == orderId);

            if (ObjectUtils.IsNotEmpty(order.DiscountId)) price = price * (order.Discount.DiscountPercent.GetValueOrDefault()/100);

            order.OrderPrice = price;
            int records = await context.SaveChangesAsync();
            if (records < 1) throw new BusinessException(ValidationAlertCode.UPDATE_RECORD_FAIL);
        }

        public async Task UpdateOrderTargetLanguage(OrderUpdate request)
        {
            var order = context.Orders
                        .Include(o => o.TargetLanguages)
                        .FirstOrDefault(o => o.OrderId == request.OrderId);

            if (order != null)
            {
                order.TargetLanguages.Clear();
                await context.SaveChangesAsync();
            }
            await AddRangeMiddle(request.OrderId, request.TargetLanguageIdList);
        }

        public async Task ChangeOrderStatus(Guid orderId, string orderStatus)
        {
            //if(OrderStatus.NEW.ToString().Equals(orderStatus) 
            //    || (UserRole.CLIENT.Equals(currentUser.Role) && !OrderStatus.CANCELLED.ToString().Equals(orderStatus))
            //    || (UserRole.STAFF.Equals(currentUser.Role) && OrderStatus.CANCELLED.ToString().Equals(orderStatus)))
            //{
            //    throw new BusinessException(AlertMessage.Alert(ValidationAlertCode.INVALID, "Order Status"));
            //}
            if(OrderStatus.ACCEPTED.ToString().Equals(orderStatus))
            {
                await context.Orders
                .Where(x => x.OrderId == orderId)
                .ExecuteUpdateAsync(x => x.SetProperty(u => u.RejectReason, (string?)null));
            }
            int records = await context.Orders
                .Where(x => x.OrderId == orderId)
                .ExecuteUpdateAsync(x => x.SetProperty(u => u.OrderStatus, orderStatus));
            if (records < 1) throw new BusinessException(ValidationAlertCode.UPDATE_RECORD_FAIL);
        }

        public async Task RecoverDeletedFiles(Guid orderId, string url)
        {
            int records = await context.OrderReferences
                .Where(x => x.OrderId == orderId && x.ReferenceFileUrl == url)
                .ExecuteUpdateAsync(x => x.SetProperty(u => u.IsDeleted, false));
            if (records < 1) throw new BusinessException(ValidationAlertCode.UPDATE_RECORD_FAIL);
        }

        public async Task DeleteOrderReferenceFile(Guid orderId, string url)
        {
            int records = await context.OrderReferences
                .Where(x => x.OrderId == orderId && x.ReferenceFileUrl.Equals(url))
                .ExecuteUpdateAsync(x => x.SetProperty(u => u.IsDeleted, true));
            if (records < 1) throw new BusinessException(ValidationAlertCode.UPDATE_RECORD_FAIL);
        }

        public async Task UploadOrderReferenceFile(List<UploadOrderFileRequest> request)
        {
            foreach(UploadOrderFileRequest one in request)
            {
                if (!Enum.IsDefined(typeof(OrderFileTag), one.Tag))
                {
                    throw new BusinessException(AlertMessage.Alert(ValidationAlertCode.INVALID, "Tag"));
                }
            }
            using (IDbContextTransaction transaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.OrderReferences.AddRange(mapper.Map<List<OrderReference>>(request));
                    int records = await context.SaveChangesAsync();
                    if (records != request.Count) throw new BusinessException(ValidationAlertCode.UPDATE_RECORD_FAIL);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            } 
        }

        public async Task<List<UploadOrderFileRequest>> GetAllOrderRefrenceFiles()
        {
            return mapper.Map<List<UploadOrderFileRequest>>(await context.OrderReferences.Where(x => !x.IsDeleted).ToListAsync());
        }

        public async Task UpdateOrderRejectResponse(ResponseRequest request)
        {
            if (await context.Orders.Where(x => x.OrderId == request.Id)
                               .ExecuteUpdateAsync(o => o.SetProperty(x => x.RejectReason, request.ResponseContent)) < 1) throw new BusinessException(ValidationAlertCode.UPDATE_RECORD_FAIL);
        }

        public async Task CreateRevelancy(Guid orderId)
        {
            using (IDbContextTransaction transaction = context.Database.BeginTransaction())
            {
                try
                {
                    List<Work> works = context.Works
                .Include(w => w.Order)
                .Include(w => w.Categories)
                .Include(w => w.Jobs)
                .ThenInclude(w => w.Assignees).ToList();
                    List<Revelancy> list = new List<Revelancy>();
                    foreach (Work work in works)
                    {
                        foreach (Job job in work.Jobs)
                        {
                            foreach (User assignee in job.Assignees)
                            {
                                foreach (Category category in work.Categories)
                                {
                                    Revelancy revelancy = new Revelancy
                                    {
                                        RevelancyId = Guid.NewGuid(),
                                        UserId = assignee.Id,
                                        SourceLanguageId = work.Order.SourceLanguageId,
                                        TargetLanguageId = job.TargetLanguageId,
                                        ServiceCode = work.ServiceCode,
                                        CategoryId = category.CategoryId
                                    };

                                    list.Add(revelancy);
                                    //context.Revelancies.Add(revelancy);
                                }
                            }
                        }
                    }

                    int count = list.Count;
                    //await context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch(Exception e)
                {
                    transaction.Rollback();
                    throw;
                }
            }
            
        }
    }
}
