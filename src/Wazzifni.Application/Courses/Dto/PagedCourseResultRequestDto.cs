using System.Collections.Generic;
using Abp.Application.Services.Dto;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Courses.Dto
{
    public class PagedCourseResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }

        public int? CityId { get; set; }

        public int? CourseCategoryId { get; set; }

        public int? TeacherId { get; set; }

        public List<int>? TagsId { get; set; }

        public bool? IsFree { get; set; }
        public bool? IsFeatured { get; set; }
        public CourseMode? Mode { get; set; }

        public CourseDifficulty? Difficulty { get; set; }

        public long? TraineeId { get; set; }

        public long? UserId { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }



    }
}
