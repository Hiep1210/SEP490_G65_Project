using AutoMapper;
using Lombok.NET;
using Microsoft.EntityFrameworkCore;
using verbum_service_application.Service;
using verbum_service_domain.Common;
using verbum_service_domain.Common.ErrorModel;
using verbum_service_domain.DTO.Request;
using verbum_service_domain.DTO.Response;
using verbum_service_domain.Models;
using verbum_service_infrastructure.DataContext;

namespace verbum_service_infrastructure.Impl.Service
{
    [RequiredArgsConstructor]
    public partial class WorkServiceImpl:WorkService
    {
        private readonly verbumContext context;
        private readonly IMapper mapper;
        private readonly CurrentUser currentUser;

        public async Task AddRange(Guid orderId, DateTime? dueDate, List<string> serviceCodes)
        {
            var works = serviceCodes.Select(serviceCode => new Work
            {
                WorkId = Guid.NewGuid(),
                OrderId = orderId,
                ServiceCode = serviceCode,
                CreatedDate = DateTime.Now,
                DueDate = dueDate
            });

            context.Works.AddRangeAsync(works);
            await context.SaveChangesAsync();
        }

        public async Task AddWorkCategory(Guid workId, List<int> categoryIds)
        {
            var categories = context.Categories.Where(c => categoryIds.Contains(c.CategoryId)).ToList();

            var work = context.Works.FirstOrDefault(x => x.WorkId == workId);
            if (work != null)
            {
                foreach (var category in categories)
                {
                    work.Categories.Add(category);
                }
            }

            await context.SaveChangesAsync();
        }

        public async Task CreateWork(Work work)
        {
            context.Works.Add(work);
            await context.SaveChangesAsync();
        }

        public async Task<List<WorkResponse>> GetAllWork()
        {
            List<Work> orders = new List<Work>();
            Guid clientId = currentUser.Id;
            switch (currentUser.Role)
            {
                case UserRole.CLIENT:
                    orders = await context.Works.ToListAsync();
                    break;
                default:
                    throw new BusinessException(AlertMessage.Alert(ValidationAlertCode.NOT_FOUND, "Role"));
            }
            List<WorkResponse> list = mapper.Map<List<WorkResponse>>(orders);
            return list;
        }

        public Task<OrderDetailsResponse> GetOrderDetails(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Guid>> GetWorkIdsListByOrderId(Guid orderId)
        {
            List<Guid> list = await context.Works
                .Where(w => w.OrderId == orderId)
                .Select(w => w.WorkId)
                .ToListAsync();

            return list;
        }

        public async Task UpdateWork(WorkUpdate request)
        {
            int records = await context.Works
                .Where(x => x.WorkId == request.WorkId)
                .ExecuteUpdateAsync(x => x.SetProperty(u => u.WorkName, request.WorkName)
                                        .SetProperty(u => u.ServiceCode, request.ServiceCode)
                                        .SetProperty(u => u.DueDate, request.DueDate));

            var work = context.Works
                        .Include(o => o.Categories)
                        .FirstOrDefault(o => o.WorkId == request.WorkId);

            if (work != null)
            {
                work.Categories.Clear();
                await context.SaveChangesAsync();
            }

            if (records < 1) throw new BusinessException(ValidationAlertCode.UPDATE_RECORD_FAIL);
        }
    }
}
