using AutoMapper;
using CloudinaryDotNet.Actions;
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
    public partial class ReceiptServiceImpl: ReceiptService
    {
        private readonly verbumContext context;
        private readonly IMapper mapper;
        private readonly CurrentUser currentUser;

        public async Task CreateReceipt(CreateReceipRequest request)
        {
            try
            {
                Receipt receipt = new Receipt();
                receipt = mapper.Map<Receipt>(request);
                receipt.PayDate = DateTime.Now;
                receipt.ReceiptId = Guid.NewGuid();
                context.Receipts.Add(receipt);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<ReceiptInfoResponse>> GetAllReceipt()
        {
            List<Receipt> listReceipt = await context.Receipts.ToListAsync();

            if (listReceipt == null)
            {
                throw new BusinessException(AlertMessage.Alert(ValidationAlertCode.NOT_FOUND, "Receipt"));
            }
            List<ReceiptInfoResponse> result = mapper.Map<List<ReceiptInfoResponse>>(listReceipt);
            return result;
        }
    }
}
