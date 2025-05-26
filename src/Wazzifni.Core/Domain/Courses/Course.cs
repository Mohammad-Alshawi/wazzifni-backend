using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Wazzifni.Domain.Cities;
using Wazzifni.Domain.CourseCategories;
using Wazzifni.Domain.CourseComments;
using Wazzifni.Domain.CourseRegistrationRequests;
using Wazzifni.Domain.CourseTags;
using Wazzifni.Domain.Teachers;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Domain.Courses
{
    public class Course : FullAuditedEntity, IMultiLingualEntity<CourseTranslation>
    {
        public double TotalHours { get; set; }
        public int RegisteredCount { get; set; }
        public CourseMode Mode { get; set; }

        public int CityId { get; set; }
        [ForeignKey(nameof(CityId))]
        public virtual City City { get; set; }
        public string LocationAddress { get; set; }

        public DateTime StartDate { get; set; }
        public CourseDifficulty Difficulty { get; set; }

        public int TeacherId { get; set; }
        [ForeignKey(nameof(TeacherId))]
        public virtual Teacher Teacher { get; set; }

        public string DailyCommitment { get; set; }

        public int CourseCategoryId { get; set; }
        [ForeignKey(nameof(CourseCategoryId))]
        public virtual CourseCategory CourseCategory { get; set; }
        public ICollection<CourseTag> Tags { get; set; }
        public bool IsFeatured { get; set; }
        public decimal? Price { get; set; }
        public double? AverageRating { get; set; }
        public ICollection<CourseComment> Comments { get; set; }

        public ICollection<CourseRegistrationRequest> CourseRegistrationRequests { get; set; }
        public ICollection<CourseTranslation> Translations { get; set; }

        [NotMapped]
        public bool IsFree => !Price.HasValue;

        public bool IsClosed { get; set; }

        public int NumberOfSeats { get; set; }

        public DateTime? ClosedDate { get; set; }
    }

}
