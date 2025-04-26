using Abp.Application.Services.Dto;

namespace Wazzifni.CourseComments.Dto
{
    public class PagedCourseCommentResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }

        public int? CourseId { get; set; }

        public long? TraineeId { get; set; }



    }
}
