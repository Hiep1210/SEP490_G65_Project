using Lombok.NET;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using verbum_service.Filter;
using verbum_service_application.Service;
using verbum_service_domain.Common;
using verbum_service_domain.Common.ErrorModel;
using verbum_service_domain.DTO.Request;
using verbum_service_domain.DTO.Response;
using verbum_service_infrastructure.Impl.Workflow;

namespace verbum_service.Controllers
{
    [Route("api/order")]
    [ApiController]
    [RequiredArgsConstructor]
    public partial class OrderController : ControllerBase
    {
        private readonly OrderService orderService;
        private readonly CreateOrderWorkflow createOrderWorkflow;
        private readonly UpdateOrderWorkflow updateOrderWorkflow;

        [HttpGet("get-all")]
        [EnableQuery]
        [Roles(UserRole.MANAGER, UserRole.CLIENT)]
        [ProducesResponseType(typeof(List<OrderResponse>), 200)]
        [ProducesResponseType(typeof(ErrorObject), 400)]
        [ProducesResponseType(500)]
        public async Task<List<OrderDetailsResponse>> GetAllOrder()
        {
            return await orderService.GetAllOrder();
        }

        [HttpGet("get-details")]
        [Authorize]
        [ProducesResponseType(typeof(OrderDetailsResponse), 200)]
        [ProducesResponseType(typeof(ErrorObject), 400)]
        [ProducesResponseType(500)]
        public async Task<OrderDetailsResponse> GetOrderDetails(Guid id)
        {
            return await orderService.GetOrderDetails(id);
        }

        [HttpPost("add")]
        [Roles(UserRole.CLIENT)]
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ErrorObject), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddOrder([FromBody] OrderCreate order)
        {
            await createOrderWorkflow.process(order);
            return NoContent();
        }

        [HttpPut("update")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorObject), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateOrder([FromBody] OrderUpdate order)
        {
            await updateOrderWorkflow.process(order);
            return NoContent();
        }

        [HttpPut("cancel")]
        [Roles(UserRole.CLIENT)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorObject), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CancelOrder(Guid orderId)
        {
            await orderService.CancelOrder(orderId);
            return NoContent();
        }

        [HttpPut("acceptordecline")]
        [Roles(UserRole.STAFF)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorObject), 400)] //if status is not accept or rejected
        [ProducesResponseType(500)]
        public async Task<IActionResult> AcceptOrDelineOrder(Guid orderId, string orderStatus)
        {
            await orderService.AcceptOrDeclineOrder(orderId, orderStatus);
            return NoContent();
        }

        [HttpGet("file")]
        [EnableQuery]
        [Roles(UserRole.MANAGER, UserRole.CLIENT)]
        [ProducesResponseType(typeof(List<UploadOrderFileRequest>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorObject), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllOrderReferenceFiles()
        {
            return ResponseFilter.OkOrNoContent(await orderService.GetAllOrderRefrenceFiles(), this);
        }

        [HttpPost("file")]
        [Roles(UserRole.MANAGER, UserRole.CLIENT)]
        [ProducesResponseType(typeof(string), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UploadOrderReferenceFile(List<UploadOrderFileRequest> request)
        {
            await orderService.UploadOrderReferenceFile(request);
            return Created();
        }
        [HttpDelete("file")]
        [Roles(UserRole.CLIENT, UserRole.MANAGER)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteOrderReferenceFile(Guid orderId, string fileURl)
        {
            await orderService.DeleteOrderReferenceFile(orderId, fileURl);
            return NoContent();
        }
        [HttpPut("file-recover")]
        [Roles(UserRole.MANAGER)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> RecoverDeletedFiles(Guid orderId, string fileURl)
        {
            await orderService.RecoverDeletedFiles(orderId, fileURl);
            return NoContent();
        }
    }
}
