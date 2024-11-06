using AutoMapper;
using Lombok.NET;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Org.BouncyCastle.Asn1.Ocsp;
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
            if (await context.SaveChangesAsync() < 1) throw new BusinessException(ValidationAlertCode.UPDATE_RECORD_FAIL);
        }

        public async Task DeleteIssueAttachmentFile(Guid issueId, string attachmentUrl)
        {
            int records = await context.IssueAttachments
                .Where(x => x.IssueId == issueId && x.AttachmentUrl == attachmentUrl)
                .ExecuteUpdateAsync(x => x.SetProperty(u => u.IsDeleted, true));
            if (records < 1) throw new BusinessException(ValidationAlertCode.UPDATE_RECORD_FAIL);
        }

        public async Task<List<UploadIssueAttachmentFiles>> GetAllIssueAttachments()
        {
            return mapper.Map<List<UploadIssueAttachmentFiles>>(await context.IssueAttachments.Where(x => !x.IsDeleted).ToListAsync());
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
            Issue? updateIssue = await context.Issues.Include(x => x.IssueAttachments).FirstOrDefaultAsync(x => x.IssueId == request.IssueId);
            updateIssue.UpdatedAt = DateTime.Now;
            updateIssue.IssueAttachments = mapper.Map<List<IssueAttachment>>(request.IssueAttachments);
            updateIssue.IssueName = request.IssueName;
            updateIssue.IssueDescription = request.IssueDescription;
            updateIssue.AssigneeId = request.AssigneeId;
            context.Issues.Update(updateIssue);
            int records = await context.SaveChangesAsync();
            if (records < 1) throw new BusinessException(ValidationAlertCode.UPDATE_RECORD_FAIL);
        }

        public async Task UpdateIssueCancelResponse(ResponseRequest request)
        {
            if (await context.Issues.Where(x => x.IssueId == request.Id)
                .ExecuteUpdateAsync(o => o.SetProperty(x => x.CancelResponse, request.ResponseContent)) < 1) throw new BusinessException(ValidationAlertCode.UPDATE_RECORD_FAIL);
        }

        public async Task UpdateIssueRejectResponse(ResponseRequest request)
        {
            if (await context.Issues.Where(x => x.IssueId == request.Id)
                .ExecuteUpdateAsync(o => o.SetProperty(x => x.RejectResponse, request.ResponseContent)) < 1) throw new BusinessException(ValidationAlertCode.UPDATE_RECORD_FAIL);
        }

        public async Task UpdateIssueStatus(Guid issueId, string status)
        {
            if (!Enum.IsDefined(typeof(IssueStatusEnum), status)
                || IssueStatusEnum.OPEN.ToString().Equals(status) 
                || (UserRole.CLIENT.Equals(currentUser.Role) && !IssueStatusEnum.CANCEL.ToString().Equals(status)))
            {
                throw new BusinessException(AlertMessage.Alert(ValidationAlertCode.INVALID, "issue status"));
            }
            if (await context.Issues.Where(x => x.IssueId.Equals(issueId)).ExecuteUpdateAsync(o => o.SetProperty(a => a.Status, status)) < 1)
            {
                throw new BusinessException(ValidationAlertCode.UPDATE_RECORD_FAIL);
            }
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

        public async Task<List<IssueResponse>> ViewAllIssue()
        {
            List<Issue> issues = await context.Issues.Include(x => x.Assignee).Include(x => x.Client).Include(x => x.Order).ToListAsync();
            switch (currentUser.Role)
            {
                case UserRole.CLIENT:
                    issues = issues
                        .Where(x => x.ClientId == currentUser.Id)
                        .ToList();
                    break;
                case UserRole.LINGUIST:
                    issues = issues
                        .Where(x => x.AssigneeId == currentUser.Id)
                        .ToList();
                    break;
                case UserRole.TRANSLATE_MANAGER:
                case UserRole.EDIT_MANAGER:
                case UserRole.EVALUATE_MANAGER:
                    break;
                default:
                    throw new BusinessException(AlertMessage.Alert(ValidationAlertCode.NOT_FOUND, "Role"));
            }
            return mapper.Map<List<IssueResponse>>(issues);
        }
    }
}
