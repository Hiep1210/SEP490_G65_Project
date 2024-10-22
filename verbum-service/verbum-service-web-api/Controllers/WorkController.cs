using Lombok.NET;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using verbum_service.Filter;
using verbum_service_application.Service;
using verbum_service_domain.Common.ErrorModel;
using verbum_service_domain.Common;
using verbum_service_domain.DTO.Response;
using verbum_service_domain.DTO.Request;
using verbum_service_infrastructure.Impl.Workflow;

namespace verbum_service.Controllers
{
    [Route("api/work")]
    [ApiController]
    [RequiredArgsConstructor]
    public partial class WorkController : ControllerBase
    {
        private readonly WorkService workService;
        private readonly CreateWorkWorkflow createWorkWorkflow;
        private readonly UpdateWorkWorkflow updateWorkWorkflow;

        [HttpGet("get-all")]
        [EnableQuery]
        //[Roles(UserRole.TRANSLATE_MANAGER,UserRole.EVALUATE_MANAGER,UserRole.EDIT_MANAGER,UserRole.MANAGER)]
        [ProducesResponseType(typeof(List<WorkResponse>), 200)]
        [ProducesResponseType(typeof(ErrorObject), 400)]
        [ProducesResponseType(500)]
        public async Task<List<WorkResponse>> GetAllWork()
        {
            return await workService.GetAllWork();
        }

        [HttpPost("add")]
        //[Roles(UserRole.TRANSLATE_MANAGER, UserRole.EVALUATE_MANAGER, UserRole.EDIT_MANAGER, UserRole.MANAGER)]
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ErrorObject), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddWork([FromBody] WorkCreate work)
        {
            await createWorkWorkflow.process(work);
            return NoContent();
        }

        [HttpPut("update")]
        //[Roles(UserRole.TRANSLATE_MANAGER, UserRole.EVALUATE_MANAGER, UserRole.EDIT_MANAGER, UserRole.MANAGER)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorObject), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateWork([FromBody] WorkUpdate work)
        {
            await updateWorkWorkflow.process(work);
            return NoContent();
        }

        [HttpPost("generate")]
        //[Roles(UserRole.TRANSLATE_MANAGER, UserRole.EVALUATE_MANAGER, UserRole.EDIT_MANAGER, UserRole.MANAGER)]
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ErrorObject), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GenerateWork([FromBody] GenerateWork request)
        {
            await workService.GenerateWork(request);
            return Created();
        }
    }
}
