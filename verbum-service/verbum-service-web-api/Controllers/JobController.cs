using Lombok.NET;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using System.ComponentModel.DataAnnotations;
using verbum_service.Filter;
using verbum_service_application.Service;
using verbum_service_domain.Common;
using verbum_service_domain.Common.ErrorModel;
using verbum_service_domain.DTO.Request;
using verbum_service_domain.DTO.Response;
using verbum_service_infrastructure.Impl.Workflow;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace verbum_service.Controllers
{
    [Route("api/job")]
    [ApiController]
    [RequiredArgsConstructor]
    public partial class JobController : ControllerBase
    {
        private readonly JobService jobService;
        [HttpGet("get-all")]
        [EnableQuery]
        [Roles(UserRole.LINGUIST, UserRole.MANAGER)]
        [ProducesResponseType(typeof(List<JobInfoResponse>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorObject), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Get()
        {
            return ResponseFilter.OkOrNoContent(await jobService.GetAllJob(), this);
        }

        [HttpPost("add")]
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ErrorObject), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateJobs([FromBody] CreateJobsRequest request)
        {
            await jobService.CreateJobs(request);
            return StatusCode(201);
        }

        [HttpPut("edit")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorObject), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateJob([FromBody] UpdateJobRequest request)
        {
            await jobService.UpdateJob(request);
            return NoContent();
        }

        [HttpPut("approve")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorObject), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ApproveJob([Required] Guid jobId, [Required] Guid orderId)
        {
            await jobService.ApproveJob(jobId,orderId);
            return NoContent();
        }
    }
}
