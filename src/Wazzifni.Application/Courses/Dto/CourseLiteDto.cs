using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using static Wazzifni.Enums.Enum;
using Wazzifni.Domain.Cities;
using Wazzifni.Domain.CourseCategories;
using Wazzifni.Domain.CourseComments;
using Wazzifni.Domain.Courses;
using Wazzifni.Domain.CourseTags;
using Wazzifni.Domain.Teachers;
using Wazzifni.Cities.Dto;
using Wazzifni.Teachers.Dto;
using Wazzifni.CourseCategories.Dto;
using Wazzifni.CourseTags.Dto;
using System.ComponentModel.DataAnnotations;

namespace Wazzifni.Courses.Dto
{
    public class CourseLiteDto : EntityDto
    {
        public int NumberOfSeats { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public double TotalHours { get; set; }
        public int RegisteredTraineesCount { get; set; }
        public CourseMode Mode { get; set; }

        public LiteCity City { get; set; }
        public string LocationAddress { get; set; }

        public DateTime StartDate { get; set; }
        public CourseDifficulty Difficulty { get; set; }

        public LiteTeacherDto Teacher { get; set; }

        public string DailyCommitment { get; set; }

        public LiteCourseCategoryDto CourseCategory { get; set; }
        public List<LiteCourseTagDto> Tags { get; set; }
        public bool IsFeatured { get; set; }
        public decimal? Price { get; set; }
        public double Rating { get; set; }
        public List<CourseTranslationDto> Translations { get; set; }

        public bool IsFree => !Price.HasValue;
        public bool IsClosed { get; set; }

        public DateTime CreationTime { get; set; }

        public LiteAttachmentDto Image { get; set; }
        public DateTime? ClosedDate { get; set; }

        public CourseRegistrationType PossibilityOfRegistration
        {
            get
            {
                if (IsClosed || StartDate > DateTime.Now)
                    return CourseRegistrationType.Special;

                if (!IsClosed || StartDate > DateTime.Now)
                    return CourseRegistrationType.Normal;

                if (IsClosed || StartDate <= DateTime.Now)
                    return CourseRegistrationType.Non;

                return CourseRegistrationType.Non;
            }
        }

    }
}
