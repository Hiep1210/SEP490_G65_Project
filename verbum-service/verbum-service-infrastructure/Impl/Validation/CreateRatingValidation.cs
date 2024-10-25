using verbum_service_application.Validation;
using verbum_service_domain.Common.ErrorModel;
using verbum_service_domain.DTO.Request;
using verbum_service_domain.Utils;
using verbum_service_infrastructure.DataContext;

namespace verbum_service_infrastructure.Impl.Validation
{
    public class CreateRatingValidation : IValidation<RatingCreate>
    {
        private readonly verbumContext context;
        public CreateRatingValidation(verbumContext context)
        {
            this.context = context;
        }

        public async Task<List<string>> Validate(RatingCreate request)
        {
            List<string> alerts = new List<string>();
            ValidateEmpty(request, alerts);
            await ValidateExist(request, alerts);
            return alerts;
        }

        private void ValidateEmpty(RatingCreate request, List<string> alerts)
        {
            
        }

        private async Task ValidateExist(RatingCreate request, List<string> alerts)
        {
            
        }
    }
}
