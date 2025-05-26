using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Wazzifni.Authorization.Users;
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

        /*      public ICollection<CourseRegistrationRequest> CourseRegistrationRequests { get; set; }

              public ICollection<CourseRate> CourseRates { get; set; }

              public ICollection<CourseComment> CourseComments { get; set; }*/

        public string EmailAddress { get; set; }

    }
}
