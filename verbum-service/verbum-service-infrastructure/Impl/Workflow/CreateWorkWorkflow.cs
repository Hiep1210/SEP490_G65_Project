using AutoMapper;
using Lombok.NET;
using System.ComponentModel.Design;
using verbum_service_application.Service;
using verbum_service_application.Workflow;
using verbum_service_domain.Common.ErrorModel;
using verbum_service_domain.Common;
using verbum_service_domain.DTO.Request;
using verbum_service_domain.Models;
using verbum_service_domain.Utils;
using verbum_service_infrastructure.Impl.Validation;

namespace verbum_service_infrastructure.Impl.Workflow
{
    [RequiredArgsConstructor]
    public partial class CreateWorkWorkflow : AbstractWorkFlow<WorkCreate>
    {
        private readonly IMapper mapper;
        private readonly CreateWorkValidation validation;
        private readonly CategoryService categoryService;
        private readonly WorkService workService;
        private Work work = new Work();
        private List<int> newIds = new List<int>(); 

        protected async override Task PreStep(WorkCreate request)
        {
        }

        protected async override Task ValidationStep(WorkCreate request)
        {
            List<string> alerts = await validation.Validate(request);
            if (ObjectUtils.IsNotEmpty(alerts))
            {
                throw new BusinessException(alerts);
            }
        }
        protected async override Task CommonStep(WorkCreate request)
        {
            work = mapper.Map<Work>(request);
            work.WorkId = Guid.NewGuid();
            work.CreatedDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
        }

        protected async override Task PostStep(WorkCreate request)
        {
            await workService.CreateWork(work);
            if(ObjectUtils.IsNotEmpty(request.OldCategoryIds)) await workService.AddWorkCategory(work.WorkId, request.OldCategoryIds);
            if (ObjectUtils.IsNotEmpty(request.NewCategory))
            {
                await categoryService.AddRange(request.NewCategory);
                newIds = await categoryService.GetListIdByCategory(request.NewCategory);
                await workService.AddWorkCategory(work.WorkId, newIds);
            }
        }
    }
}
