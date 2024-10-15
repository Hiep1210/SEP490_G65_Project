using AutoMapper;
using Lombok.NET;
using verbum_service_application.Service;
using verbum_service_application.Workflow;
using verbum_service_domain.Common.ErrorModel;
using verbum_service_domain.DTO.Request;
using verbum_service_domain.Models;
using verbum_service_domain.Utils;
using verbum_service_infrastructure.Impl.Validation;

namespace verbum_service_infrastructure.Impl.Workflow
{
    [RequiredArgsConstructor]
    public partial class UpdateWorkWorkflow : AbstractWorkFlow<WorkUpdate>
    {
        private readonly IMapper mapper;
        private readonly UpdateWorkValidation validation;
        private readonly WorkService workService;
        private readonly CategoryService categoryService;
        private List<int> newIds = new List<int>();

        protected async override Task PreStep(WorkUpdate request)
        {
        }

        protected async override Task ValidationStep(WorkUpdate request)
        {
            List<string> alerts = await validation.Validate(request);
            if (ObjectUtils.IsNotEmpty(alerts))
            {
                throw new BusinessException(alerts);
            }
        }
        protected async override Task CommonStep(WorkUpdate request)
        {

        }

        protected async override Task PostStep(WorkUpdate request)
        {
            await workService.UpdateWork(request);
            if (ObjectUtils.IsNotEmpty(request.OldCategoryIds)) await workService.AddWorkCategory(request.WorkId, request.OldCategoryIds);
            if (ObjectUtils.IsNotEmpty(request.NewCategory))
            {
                await categoryService.AddRange(request.NewCategory);
                newIds = await categoryService.GetListIdByCategory(request.NewCategory);
                await workService.AddWorkCategory(request.WorkId, newIds);
            }
        }
    }
}
