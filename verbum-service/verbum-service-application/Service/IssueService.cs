using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using verbum_service_domain.Common;
using verbum_service_domain.DTO.Request;
using verbum_service_domain.Models;

namespace verbum_service_application.Service
{
    public interface IssueService
    {
        Task AddIssue(CreateIssueRequest request);
        Task<List<Issue>> ViewAllIssue();
        Task UpdateIssue(UpdateIssueRequest request);
        Task UploadIssueAttachment(List<UploadIssueAttachmentFiles> attachmentFiles);
        Task RecoverDeletedFiles(Guid issueId, string attachmentUrl);
        Task DeleteIssueAttachmentFile(Guid issueId, string attachmentUrl);
    }
}
