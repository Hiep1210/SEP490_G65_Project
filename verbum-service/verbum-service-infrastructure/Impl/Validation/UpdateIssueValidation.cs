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
using verbum_service_domain.Utils;
using verbum_service_infrastructure.DataContext;

namespace verbum_service_infrastructure.Impl.Validation
{
    [RequiredArgsConstructor]
    public partial class UpdateIssueValidation : IValidation<UpdateIssueRequest>
    {
        private readonly verbumContext context;
        private readonly CurrentUser currentUser;
        public async Task<List<string>> Validate(UpdateIssueRequest request)
        {
            List<string> errors = new();
            if (ObjectUtils.IsEmpty(request.IssueName) || ObjectUtils.IsEmpty(request.IssueDescription))
            {
                errors.Add(AlertMessage.Alert(ValidationAlertCode.REQUIRED, "missing fields"));
                return errors;
            }
            //if (!IssueStatusEnum.OPEN.ToString().Equals(await context.Issues.Where(x => x.IssueId == request.IssueId).Select(x => x.Status).FirstOrDefaultAsync())) 
            //{
            //    errors.Add(AlertMessage.Alert(ValidationAlertCode.CANNOT_UPDATE, "issue because issue is not open anymore"));
            //}
            foreach (var attachment in request.IssueAttachments)
            {
                if (!Enum.IsDefined(typeof(IssueFileTag), attachment.Tag))
                {
                    errors.Add(AlertMessage.Alert(ValidationAlertCode.INVALID, "one of issue attachment's tag"));
                }
            }
            if (await context.Issues.AnyAsync(x => x.IssueName.Equals(request.IssueName) && x.IssueId != request.IssueId))
            {
                errors.Add(AlertMessage.Alert(ValidationAlertCode.DUPLICATE, "issue name"));
            }
            return errors;
        }
    }
}
