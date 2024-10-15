using Microsoft.EntityFrameworkCore;
using verbum_service_application.Validation;
using verbum_service_domain.Common.ErrorModel;
using verbum_service_domain.DTO.Request;
using verbum_service_infrastructure.DataContext;

namespace verbum_service_infrastructure.Impl.Validation
{
    public class UpdateWorkValidation : IValidation<WorkUpdate>
    {
        private readonly verbumContext context;
        public UpdateWorkValidation(verbumContext context)
        {
            this.context = context;
        }

        public async Task<List<string>> Validate(WorkUpdate request)
        {
            List<string> alerts = new List<string>();
            ValidateEmpty(request, alerts);
            await ValidateExist(request, alerts);
            return alerts;
        }

        private void ValidateEmpty(WorkUpdate request, List<string> alerts)
        {

        }

        private async Task ValidateExist(WorkUpdate request, List<string> alerts)
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
