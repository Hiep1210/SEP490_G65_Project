using verbum_service_application.Validation;
using verbum_service_domain.DTO.Request;
using verbum_service_infrastructure.DataContext;

namespace verbum_service_infrastructure.Impl.Validation
{
    public class UpdateRatingValidation : IValidation<RatingUpdate>
    {
        private readonly verbumContext context;
        public UpdateRatingValidation(verbumContext context)
        {
            this.context = context;
        }

        public async Task<List<string>> Validate(RatingUpdate request)
        {
            List<string> alerts = new List<string>();
            ValidateEmpty(request, alerts);
            await ValidateExist(request, alerts);
            return alerts;
        }

        private void ValidateEmpty(RatingUpdate request, List<string> alerts)
        {

        }

        private async Task ValidateExist(RatingUpdate request, List<string> alerts)
        {

        }
    }
}
