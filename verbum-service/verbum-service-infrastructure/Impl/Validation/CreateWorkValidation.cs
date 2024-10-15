using Microsoft.EntityFrameworkCore;
using verbum_service_application.Validation;
using verbum_service_domain.Common.ErrorModel;
using verbum_service_domain.DTO.Request;
using verbum_service_domain.Utils;
using verbum_service_infrastructure.DataContext;

namespace verbum_service_infrastructure.Impl.Validation
{
    public class CreateWorkValidation : IValidation<WorkCreate>
    {
        private readonly verbumContext context;
        public CreateWorkValidation(verbumContext context)
        {
            this.context = context;
        }

        public async Task<List<string>> Validate(WorkCreate request)
        {
            List<string> alerts = new List<string>();
            ValidateEmpty(request, alerts);
            await ValidateExist(request, alerts);
            return alerts;
        }

        private void ValidateEmpty(WorkCreate request, List<string> alerts)
        {
            int count = 0;
            if (ObjectUtils.IsNotEmpty(request.OldCategoryIds)) count += request.OldCategoryIds.Count();
            if (ObjectUtils.IsNotEmpty(request.OldCategoryIds)) count += request.OldCategoryIds.Count();

            if(count > 3)
            {
                alerts.Add(AlertMessage.Alert(ValidationAlertCode.INVALID, "Work can only have max of 3 categories"));
            }
            if(count < 1)
            {
                alerts.Add(AlertMessage.Alert(ValidationAlertCode.INVALID, "Work must have 1 category"));
            }
        }

        private async Task ValidateExist(WorkCreate request, List<string> alerts)
        {
            if (await context.Categories.AnyAsync(c => request.NewCategory.Contains(c.CategoryName)))
            {
                alerts.Add(AlertMessage.Alert(ValidationAlertCode.DUPLICATE, "NewCategory"));
            }
            if (!request.OldCategoryIds.All(id => context.Categories.Any(c => c.CategoryId == id)))
            {
                alerts.Add(AlertMessage.Alert(ValidationAlertCode.NOT_FOUND, "Category"));
            }
        }

    }
}
