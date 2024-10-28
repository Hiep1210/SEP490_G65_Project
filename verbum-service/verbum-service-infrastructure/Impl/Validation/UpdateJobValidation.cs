using Lombok.NET;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using verbum_service_application.Validation;
using verbum_service_domain.Common;
using verbum_service_domain.Common.ErrorModel;
using verbum_service_domain.DTO.Request;
using verbum_service_infrastructure.DataContext;

namespace verbum_service_infrastructure.Impl.Validation
{
    [RequiredArgsConstructor]
    public partial class UpdateJobValidation : IValidation<UpdateJobRequest>
    {
        private readonly verbumContext context;
        public async Task<List<string>> Validate(UpdateJobRequest request)
        {
            List<string> errors = new List<string>();
            if (!Enum.IsDefined(typeof(JobStatus), request.Status))
            {
                errors.Add(AlertMessage.Alert(ValidationAlertCode.INVALID, "job status"));
            }
            if (await context.Jobs.AnyAsync(x => x.Name.Equals(request.Name) && x.Id != request.Id))
            {
                errors.Add(AlertMessage.Alert(ValidationAlertCode.DUPLICATE, "job name"));
            }
            return errors;
        }
    }
}
