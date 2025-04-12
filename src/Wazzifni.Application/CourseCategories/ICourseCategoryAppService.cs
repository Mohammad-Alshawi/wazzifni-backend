using KeyFinder;
using System.Threading.Tasks;
using Wazzifni.Cities.Dto;
using Wazzifni.CourseCategories.Dto;
using Wazzifni.CrudAppServiceBase;

namespace Wazzifni.CourseCategories
{
    public interface ICourseCategoryAppService : IWazzifniAsyncCrudAppService<CourseCategoryDetailsDto, int, LiteCourseCategoryDto, PagedCourseCategoryResultRequestDto,
        CreateCourseCategoryDto, UpdateCourseCategoryDto>
    {
        Task<CourseCategoryDetailsDto> SwitchActivationAsync(SwitchActivationInputDto input);

    }
}
