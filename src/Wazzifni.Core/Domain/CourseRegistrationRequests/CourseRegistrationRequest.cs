using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Wazzifni.Authorization.Users;
using Wazzifni.Domain.Courses;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Domain.CourseRegistrationRequests
{
    public class CourseRegistrationRequest : FullAuditedEntity<long>
    {
        public int CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]
        public virtual Course Course { get; set; }

        /*       public long TraineeId { get; set; }
               [ForeignKey(nameof(TraineeId))]
               public virtual Trainee Trainee { get; set; }*/


        public long UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        public CourseRegistrationRequestStatus? Status { get; set; }


        public bool IsSpecial { get; set; }

        public string RejectReason { get; set; }

        public int NumberOfRegisteredPeople { get; set; }

    }
}
