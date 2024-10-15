using AutoMapper;
using Lombok.NET;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Org.BouncyCastle.Asn1.Ocsp;
using verbum_service_application.Service;
using verbum_service_domain.Common;
using verbum_service_domain.Common.ErrorModel;
using verbum_service_domain.DTO.Request;
using verbum_service_domain.Models;
using verbum_service_infrastructure.DataContext;

namespace verbum_service_infrastructure.Impl.Service
{
    [RequiredArgsConstructor]
    public partial class IssueServiceImpl : IssueService
    {
        private readonly IMapper mapper;
        private readonly verbumContext context;
        private readonly CurrentUser currentUser;
        public async Task AddIssue(CreateIssueRequest request)
        {
            Issue issue = mapper.Map<Issue>(request);
            issue.IssueId = Guid.NewGuid();
            issue.CreatedAt = DateTime.Now;
            issue.UpdatedAt = DateTime.Now;
            issue.Status = IssueStatusEnum.OPEN.ToString();
            issue.ClientId = currentUser.Id;
            context.Issues.Add(issue);
            await context.SaveChangesAsync();
        }

        public async Task DeleteIssueAttachmentFile(Guid issueId, string attachmentUrl)
        {
            int records = await context.IssueAttachments
                .Where(x => x.IssueId == issueId && x.AttachmentUrl == attachmentUrl)
                .ExecuteUpdateAsync(x => x.SetProperty(u => u.IsDeleted, true));
            if (records < 1) throw new BusinessException(ValidationAlertCode.UPDATE_RECORD_FAIL);
        }

        public async Task RecoverDeletedFiles(Guid issueId, string attachmentUrl)
        {
            int records = await context.IssueAttachments
                .Where(x => x.IssueId == issueId && x.AttachmentUrl == attachmentUrl)
                .ExecuteUpdateAsync(x => x.SetProperty(u => u.IsDeleted, false));
            if (records < 1) throw new BusinessException(ValidationAlertCode.UPDATE_RECORD_FAIL);
        }

        public async Task UpdateIssue(UpdateIssueRequest request)
        {
            int records = await context.Issues
                .Where(x => x.IssueId == request.IssueId)
                .ExecuteUpdateAsync(x => x.SetProperty(u => u.UpdatedAt, DateTime.Now)
                                        .SetProperty(u => u.IssueName, request.IssueName)
                                        .SetProperty(u => u.IssueDescription, request.IssueDescription));
            if (records < 1) throw new BusinessException(ValidationAlertCode.UPDATE_RECORD_FAIL);
        }

        public async Task UploadIssueAttachment(List<UploadIssueAttachmentFiles> attachmentFiles)
        {
            using (IDbContextTransaction transaction = context.Database.BeginTransaction())
            {
                try
                {
                    context.IssueAttachments.AddRange(mapper.Map<List<IssueAttachment>>(attachmentFiles));
                    int records = await context.SaveChangesAsync();
                    if (records != attachmentFiles.Count) throw new BusinessException(ValidationAlertCode.UPDATE_RECORD_FAIL);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<List<Issue>> ViewAllIssue()
        {
            List<Issue> issues = new List<Issue>();
            switch(currentUser.Role)
            {
                case UserRole.CLIENT:
                    issues = await context.Issues
                        .Where(x => x.ClientId == currentUser.Id)
                        .ToListAsync();
                    break;
                case UserRole.LINGUIST:
                    issues = await context.Issues
                        .Where(x => x.AssigneeId == currentUser.Id)
                        .ToListAsync();
                    break;
                case UserRole.TRANSLATE_MANAGER:
                case UserRole.EDIT_MANAGER:
                case UserRole.EVALUATE_MANAGER:
                    issues = await context.Issues.ToListAsync();
                    break;
                default:
                    throw new BusinessException(AlertMessage.Alert(ValidationAlertCode.NOT_FOUND, "Role"));
            }
            return issues;
        }
    }
}
