using AutoMapper;
using Lombok.NET;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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
                        Work work = await context.Works.FirstOrDefaultAsync(w => w.WorkId == workId);
                        foreach (CreateJobFileUpload fileUpload in request.UploadFiles)
                        {
                            foreach (string targetLangId in request.TargetLanguageIds)
                            {
                                Job job = new Job
                                {
                                    Id = Guid.NewGuid(),
                                    Name = "Job_" + targetLangId + "_" + work.ServiceCode + "_" + fileUpload.DocumentUrl.Split("/")[^1].Split(".docx")[0],
                                    Status = JobStatus.NEW.ToString(),
                                    DueDate = work.DueDate,
                                    CreatedAt = DateTime.Now,
                                    UpdatedAt = DateTime.Now,
                                    WordCount = fileUpload.WordCount,
                                    WorkId = workId,
                                    DocumentUrl = fileUpload.DocumentUrl,
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

        public async Task<List<JobInfoResponse>> GetAllJob()
        {
            return mapper.Map<List<JobInfoResponse>>(await context.Jobs.Include(x => x.Assignees).ToListAsync());
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
            List<User> newAssignees = request.AssigneesId.Select(userId => new User { Id = userId }).ToList();
            job.Assignees = await context.Users
                .Where(user => request.AssigneesId.Contains(user.Id))
                .ToListAsync();
            if (await context.SaveChangesAsync() < 1)
            {
                throw new BusinessException(ValidationAlertCode.UPDATE_RECORD_FAIL);
            }
        }
    }
}
