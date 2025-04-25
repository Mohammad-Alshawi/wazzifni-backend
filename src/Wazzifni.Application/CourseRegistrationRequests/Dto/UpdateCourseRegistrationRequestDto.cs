using Abp.Application.Services.Dto;
using Wazzifni.CourseRegistrationRequests.Dto;

namespace Wazzifni.CourseRegistrationRequests.Dto
{
    public class UpdateCourseRegistrationRequestDto : CreateCourseRegistrationRequestDto, IEntityDto<long>
    {
        public long Id { get; set; }
    }
}
