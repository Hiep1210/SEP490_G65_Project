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
using verbum_service_domain.Utils;
using verbum_service_infrastructure.DataContext;
using verbum_service_infrastructure.Impl.Validation;

namespace verbum_service_infrastructure.Impl.Service
{
    [RequiredArgsConstructor]
    public partial class JobServiceImpl : JobService
    {
        private readonly verbumContext context;
        private readonly IMapper mapper;
        private readonly UpdateJobValidation validation;
        public async Task CreateJobs(CreateJobsRequest request)
        {
            using (IDbContextTransaction transaction = context.Database.BeginTransaction())
            {
                try
                {
                    foreach (Guid workId in request.WorkIds)
                    {
                        Work work = await context.Works.Include(x => x.ServiceCodeNavigation).FirstOrDefaultAsync(w => w.WorkId == workId);
                        foreach (string docUrl in request.DocumentUrls)
                        {
                            foreach (string targetLangId in request.TargetLanguageIds)
                            {
                                Job job = new Job
                                {
                                    Id = Guid.NewGuid(),
                                    Name = targetLangId + "_" + work.ServiceCodeNavigation.ServiceName + "_" + docUrl.Split("/")[^1].Split(".docx")[0].Replace("%20", " "),
                                    Status = JobStatus.NEW.ToString(),
                                    CreatedAt = DateTime.Now,
                                    UpdatedAt = DateTime.Now,
                                    WordCount = 0,
                                    WorkId = workId,
                                    DocumentUrl = docUrl,
                                    TargetLanguageId = targetLangId
                                };
                                context.Jobs.Add(job);
                            }
                        }
                    }
                    await context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<List<JobListResponse>> GetAllJob()
        {
            List<JobListResponse> allJobs = mapper.Map<List<JobListResponse>>(await context.Jobs.Include(x => x.Assignees).ToListAsync()); 
            return allJobs;
        }

        public async Task<JobInfoResponse> GetJobById(Guid jobId)
        {
            JobInfoResponse job = mapper.Map<JobInfoResponse>(await context.Jobs.Include(x => x.Assignees).Include(x => x.Issue).Include(x => x.Work).FirstOrDefaultAsync(x => x.Id.Equals(jobId)));
            List<string> urls = await context.Jobs.Include(x => x.Work).ThenInclude(x => x.ServiceCodeNavigation).Where(x => x.DocumentUrl.Equals(job.DocumentUrl) && x.Id != job.Id).OrderBy(x => x.Work.ServiceCodeNavigation.ServiceOrder).Select(x => x.DeliverableUrl).ToListAsync();
            job.PreviousJobDeliverables = urls;
            return job;
        }

        public async Task UpdateJob(UpdateJobRequest request)
        {
            List<string> errors = await validation.Validate(request);
            if (ObjectUtils.IsNotEmpty(errors))
            {
                throw new BusinessException(errors);
            }
            Job job = await context.Jobs.Include(x => x.Assignees).FirstOrDefaultAsync(x => x.Id.Equals(request.Id));
            job.Name = request.Name;
            job.Status = request.Status;
            job.DeliverableUrl = request.DeliverableUrl;
            job.DueDate = request.DueDate;
            List<User> newAssignees = request.AssigneesId.Select(userId => new User { Id = userId }).ToList();
            job.Assignees = await context.Users
                .Where(user => request.AssigneesId.Contains(user.Id))
                .ToListAsync();
            if (await context.SaveChangesAsync() < 1)
            {
                throw new BusinessException(ValidationAlertCode.UPDATE_RECORD_FAIL);
            }
        }

        public async Task ApproveJob(Guid jobId)
        {
            int jobRecords = await context.Jobs
                .Where(x => x.Id == jobId)
                .ExecuteUpdateAsync(x => x.SetProperty(u => u.Status, JobStatus.APPROVED.ToString()));

            Guid orderId = await context.Jobs.Where(x => x.Id == jobId)
                        .Include(x => x.Work)
                        .ThenInclude(x => x.Order).Select(x => x.Work.Order.OrderId).FirstOrDefaultAsync();

            if (jobRecords < 1) throw new BusinessException(ValidationAlertCode.UPDATE_RECORD_FAIL);

            var jobs = await context.Jobs
                .Include(x => x.Work)
                .Include(x => x.Issue)
                .Where(x => x.Work.OrderId == orderId)
                .ToListAsync();

            bool allCompleted = jobs.All(job =>
            job.Status == JobStatus.APPROVED.ToString() &&
            (job.Issue == null || job.Issue.Status == IssueStatusEnum.RESOLVED.ToString() || job.Issue.Status == IssueStatusEnum.CANCEL.ToString()));

            if (allCompleted)
            {
                int orderRecords = await context.Orders
                    .Where(o => o.OrderId == orderId)
                    .ExecuteUpdateAsync(x => x.SetProperty(u => u.OrderStatus, OrderStatus.COMPLETED.ToString()));

                if (orderRecords < 1) throw new BusinessException(ValidationAlertCode.UPDATE_RECORD_FAIL);
            }
        }

    }
}
