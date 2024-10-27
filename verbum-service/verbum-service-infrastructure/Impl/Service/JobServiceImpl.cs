using AutoMapper;
using Lombok.NET;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using verbum_service_application.Service;
using verbum_service_domain.Common;
using verbum_service_domain.DTO.Request;
using verbum_service_domain.DTO.Response;
using verbum_service_domain.Models;
using verbum_service_infrastructure.DataContext;

namespace verbum_service_infrastructure.Impl.Service
{
    [RequiredArgsConstructor]
    public partial class JobServiceImpl : JobService
    {
        private readonly verbumContext context;
        private readonly IMapper mapper;
        public async Task CreateJobs(CreateJobsRequest request)
        {
            using (IDbContextTransaction transaction = context.Database.BeginTransaction())
            {
                try
                {
                    foreach (Guid workId in request.WorkIds)
                    {
                        Work work = await context.Works.FirstOrDefaultAsync(w => w.WorkId == workId);
                        foreach (string docUrl in request.DocumentUrls)
                        {
                            foreach (string targetLangId in request.TargetLanguageIds)
                            {
                                Job job = new Job
                                {
                                    Id = Guid.NewGuid(),
                                    Name = "Job_" + targetLangId + "_" + work.ServiceCode + "_" + docUrl.Split("/")[^1].Split(".docx")[0],
                                    Status = JobStatus.NEW.ToString(),
                                    DueDate = work.DueDate,
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
                } catch {
                    transaction.Rollback();
                    throw;
                }
            } 
        }

        public async Task<List<JobInfoResponse>> GetAllJob()
        {
            return mapper.Map<List<JobInfoResponse>>(await context.Jobs.ToListAsync());
        }
    }
}
