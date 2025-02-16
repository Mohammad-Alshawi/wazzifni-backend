using Wazzifni.CrudAppServiceBase;
using Wazzifni.WorkApplications.Dto;

namespace Wazzifni.WorkApplications
{
    public interface IWorkApplicationAppService : IWazzifniAsyncCrudAppService<WorkApplicationDetailsDto, long, WorkApplicationLiteDto, PagedWorkApplicationResultRequestDto,
        CreateWorkApplicationDto, UpdateWorkApplicationDto>
    {
    }
}
