using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Wazzifni.Enums.Enum;
using Wazzifni.Cities.Dto;
using Wazzifni.CourseCategories.Dto;
using Wazzifni.CourseTags.Dto;
using Wazzifni.Teachers.Dto;

namespace Wazzifni.Courses.Dto
{
    public class CreateCourseDto
    {
        public int NumberOfSeats { get; set; }

        public double TotalHours { get; set; }
        public CourseMode Mode { get; set; }
        public int? CityId { get; set; }
        public string LocationAddress { get; set; }
        public DateTime StartDate { get; set; }
        public CourseDifficulty Difficulty { get; set; }
        public int TeacherId { get; set; }
        public string DailyCommitment { get; set; }
        public int CourseCategoryId { get; set; }
        public List<int> TagsIds { get; set; }
        public bool IsFeatured { get; set; }
        public decimal? Price { get; set; }
        public List<CourseTranslationDto> Translations { get; set; }

        public List<long> Attachments { get; set; } = new List<long>();


    }
}
