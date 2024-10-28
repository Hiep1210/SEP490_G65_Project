using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using verbum_service_domain.DTO.Request;
using verbum_service_domain.DTO.Response;

namespace verbum_service_application.Service
{
    public interface JobService
    {
        Task<List<JobInfoResponse>> GetAllJob();
        Task CreateJobs(CreateJobsRequest request);
        Task UpdateJob(UpdateJobRequest request);
    }
}
