using KeyFinder;
using System.Threading.Tasks;
using Wazzifni.CourseTags.Dto;
using Wazzifni.CrudAppServiceBase;
using Wazzifni.Universities.Dto;

namespace Wazzifni.CourseTags
{
    public interface ICourseTagAppService : IWazzifniAsyncCrudAppService<CourseTagDetailsDto, int, LiteCourseTagDto, PagedCourseTagResultRequestDto,
        CreateCourseTagDto, UpdateCourseTagDto>
    {
        Task<CourseTagDetailsDto> SwitchActivationAsync(SwitchActivationInputDto input);

    }
}
