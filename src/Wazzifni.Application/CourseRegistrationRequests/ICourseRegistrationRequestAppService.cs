using Wazzifni.CrudAppServiceBase;
using Wazzifni.CourseRegistrationRequests.Dto;

namespace Wazzifni.CourseRegistrationRequests
{
    public interface ICourseRegistrationRequestAppService : IWazzifniAsyncCrudAppService<CourseRegistrationRequestDetailsDto, long, CourseRegistrationRequestLiteDto, PagedCourseRegistrationRequestResultRequestDto,
        CreateCourseRegistrationRequestDto, UpdateCourseRegistrationRequestDto>
    {
    }
}
