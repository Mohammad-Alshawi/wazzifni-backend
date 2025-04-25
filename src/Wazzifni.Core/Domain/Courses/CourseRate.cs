using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using Wazzifni.Authorization.Users;
using Wazzifni.Domain.Trainees;

namespace Wazzifni.Domain.Courses
{
    public class CourseRate : FullAuditedEntity<long>
    {

        [ForeignKey(nameof(CourseId))]
        public virtual Course Course { get; set; }
        public int? CourseId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
        public long UserId { get; set; }

        [ForeignKey(nameof(TraineeId))]
        public virtual Trainee Trainee { get; set; }
        public long? TraineeId { get; set; }


        public double Rate { get; set; }
    }
}
