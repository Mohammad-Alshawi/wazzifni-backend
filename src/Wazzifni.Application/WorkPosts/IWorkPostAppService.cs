using Wazzifni.CrudAppServiceBase;
using Wazzifni.WorkPosts.Dto;

namespace Wazzifni.WorkPosts
{
    public interface IWorkPostAppService : IWazzifniAsyncCrudAppService<WorkPostDetailsDto, long, WorkPostLiteDto, PagedWorkPostResultRequestDto,
        CreateWorkPostDto, UpdateWorkPostDto>
    {
    }
}
