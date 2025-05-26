using Abp.Application.Services.Dto;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.CourseRegistrationRequests.Dto
{
    public class PagedCourseRegistrationRequestResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }

        public long? CourseId { get; set; }

        public CourseRegistrationRequestStatus? Status { get; set; }

        public long? TraineeId { get; set; }
        public long? UserId { get; set; }

        public bool? IsSpecial { get; set; }



    }
}
