using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using Wazzifni.Authorization.Users;
using Wazzifni.Domain.CourseComments;
using Wazzifni.Domain.CourseRegistrationRequests;
using Wazzifni.Domain.Courses;
using Wazzifni.Domain.Universities;

namespace Wazzifni.Domain.Trainees
{
    public class Trainee : FullAuditedEntity<long>
    {
        public long UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        public int? UniversityId { get; set; }
        [ForeignKey(nameof(UniversityId))]
        public virtual University University { get; set; }

        public string UniversityMajor { get; set; }

        public ICollection<CourseRegistrationRequest> CourseRegistrationRequests { get; set; }

        public ICollection<CourseRate> CourseRates { get; set; }

        public ICollection<CourseComment> CourseComments { get; set; }



    }
}
