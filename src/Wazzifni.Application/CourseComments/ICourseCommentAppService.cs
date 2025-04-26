using KeyFinder;
using System.Threading.Tasks;
using Wazzifni.Cities.Dto;
using Wazzifni.CourseCategories.Dto;
using Wazzifni.CourseComments.Dto;
using Wazzifni.CrudAppServiceBase;

namespace Wazzifni.CourseComments
{
    public interface ICourseCommentAppService : IWazzifniAsyncCrudAppService<CourseCommentDetailsDto, long, CourseCommentLiteDto, PagedCourseCommentResultRequestDto,
        CreateCourseCommentDto, UpdateCourseCommentDto>
    {

    }
}
