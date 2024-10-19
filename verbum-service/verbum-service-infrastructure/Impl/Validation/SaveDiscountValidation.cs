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
using verbum_service_domain.Utils;
using verbum_service_infrastructure.DataContext;

namespace verbum_service_infrastructure.Impl.Validation
{
    [RequiredArgsConstructor]
    public partial class SaveDiscountValidation : IValidation<DiscountDTO>
    {
        private readonly verbumContext context;
        public async Task<List<string>> Validate(DiscountDTO request)
        {
            List<string> errors = new List<string>();
            errors.AddRange(ValidateRequired(request));
            if (ObjectUtils.IsNotEmpty(errors))
            {
                return errors;
            }
            errors.AddRange(await ValidateDuplicate(request));
            return errors;
        }
        public List<string> ValidateRequired(DiscountDTO request)
        {
            List<string> errors = new List<string>();
            if(ObjectUtils.IsEmpty(request.DiscountPercent))
            {
                errors.Add(AlertMessage.Alert(ValidationAlertCode.REQUIRED, "discount percent"));
            }
            if(ObjectUtils.IsEmpty(request.DiscountName))
            {
                errors.Add(AlertMessage.Alert(ValidationAlertCode.REQUIRED, "discount name"));
            }
            return errors;
        }
        public async Task<List<string>> ValidateDuplicate(DiscountDTO request)
        {
            List<string> errors = new List<string>();
            if (request.IsUpdate)
            {
                if (await context.Discounts.AnyAsync(x => x.DiscountName == request.DiscountName && x.DiscountId != request.DiscountId))
                {
                    errors.Add(AlertMessage.Alert(ValidationAlertCode.DUPLICATE, "discount name"));
                }
                if (await context.Discounts.AnyAsync(context => context.DiscountPercent == request.DiscountPercent && context.DiscountId != request.DiscountId))
                {
                    errors.Add(AlertMessage.Alert(ValidationAlertCode.DUPLICATE, "discount percent"));
                }
            } else
            {
                if (await context.Discounts.AnyAsync(x => x.DiscountName == request.DiscountName ))
                {
                    errors.Add(AlertMessage.Alert(ValidationAlertCode.DUPLICATE, "discount name"));
                }
                if (await context.Discounts.AnyAsync(context => context.DiscountPercent == request.DiscountPercent ))
                {
                    errors.Add(AlertMessage.Alert(ValidationAlertCode.DUPLICATE, "discount percent"));
                }
            }
            return errors;
        }
    }
}
