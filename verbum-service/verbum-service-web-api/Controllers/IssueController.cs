using Lombok.NET;
using Microsoft.AspNetCore.Mvc;
using verbum_service.Filter;
using verbum_service_application.Service;
using verbum_service_domain.Common;
using verbum_service_domain.Common.ErrorModel;
using verbum_service_domain.DTO.Request;
using verbum_service_domain.Models;
using verbum_service_infrastructure.Impl.Workflow;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace verbum_service.Controllers
{
    [Route("api/issue")]
    [ApiController]
    [RequiredArgsConstructor]
    public partial class IssueController : ControllerBase
    {
        private readonly IssueService issueService;
        private readonly CreateIssueWorkflow createIssueWorkflow;
        private readonly UpdateIssueWorkflow updateIssueWorkflow;
        [HttpGet]
        [Roles(UserRole.MANAGER, UserRole.LINGUIST, UserRole.CLIENT)]
        [ProducesResponseType(typeof(List<Issue>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorObject), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ViewAllIssue()
        {
            return ResponseFilter.OkOrNoContent(await issueService.ViewAllIssue(), this);
        }
        [HttpPost]
        [Roles(UserRole.CLIENT)]
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ErrorObject), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddIssue(CreateIssueRequest request)
        {
            await createIssueWorkflow.process(request);
            return Created();
        }

        [HttpPut]
        [Roles(UserRole.CLIENT)]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ErrorObject), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateIssue(UpdateIssueRequest request)
        {
            await updateIssueWorkflow.process(request);
            return NoContent();
        }

        [HttpGet("file")]
        [Roles(UserRole.CLIENT, UserRole.MANAGER)]
        [ProducesResponseType(typeof(List<UploadIssueAttachmentFiles>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllIssueAttachments()
        {
            return ResponseFilter.OkOrNoContent(await issueService.GetAllIssueAttachments(), this);
        }

        [HttpPost("file")]
        [Roles(UserRole.CLIENT, UserRole.MANAGER)]
        [ProducesResponseType(typeof(string), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UploadIssueAttachment(List<UploadIssueAttachmentFiles> attachmentFiles)
        {
            await issueService.UploadIssueAttachment(attachmentFiles);
            return Created();
        }

        [HttpDelete("file")]
        [Roles(UserRole.MANAGER, UserRole.CLIENT)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteIssueAttachmentFile(Guid issueId, string fileURl)
        {
            await issueService.DeleteIssueAttachmentFile(issueId, fileURl);
            return NoContent();
        }

        [HttpPut("file-recover")]
        [Roles(UserRole.MANAGER)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> RecoverDeletedFiles(Guid issueId, string fileURl)
        {
            await issueService.RecoverDeletedFiles(issueId, fileURl);
            return NoContent();
        }
    }
}
