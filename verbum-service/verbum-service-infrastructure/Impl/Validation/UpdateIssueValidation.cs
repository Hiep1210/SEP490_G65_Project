using Lombok.NET;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using verbum_service_application.Validation;
using verbum_service_domain.Common.ErrorModel;
using verbum_service_domain.DTO.Request;
using verbum_service_infrastructure.DataContext;

namespace verbum_service_infrastructure.Impl.Validation
{
    [RequiredArgsConstructor]
    public partial class UpdateIssueValidation : IValidation<UpdateIssueRequest>
    {
        private readonly verbumContext context;
        public async Task<List<string>> Validate(UpdateIssueRequest request)
        {
            List<string> errors = new List<string>();
            if(await context.Issues.AnyAsync(x => x.IssueName.Equals(request.IssueName) && x.IssueId != request.IssueId))
            {
                errors.Add(AlertMessage.Alert(ValidationAlertCode.DUPLICATE, "issue name"));
            }
            return errors;
        }
    }
}
