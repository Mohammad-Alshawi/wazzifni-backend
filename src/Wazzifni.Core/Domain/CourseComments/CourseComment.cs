using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Wazzifni.Authorization.Users;
using Wazzifni.Domain.Courses;

namespace Wazzifni.Domain.CourseComments
{
    public class CourseComment : FullAuditedEntity<long>
    {
        /*       public long TraineeId { get; set; }
               [ForeignKey(nameof(TraineeId))]
               public virtual Trainee Trainee { get; set; }*/



        public long UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        public int CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]
        public virtual Course Course { get; set; }

        public string Content { get; set; }

    }
}
