using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using Wazzifni.Domain.Cities;
using Wazzifni.Domain.Courses;
using Wazzifni.Domain.Trainees;

namespace Wazzifni.Domain.CourseComments
{
    public class CourseComment : FullAuditedEntity<long>
    {
        public long TraineeId { get; set; }
        [ForeignKey(nameof(TraineeId))]
        public virtual Trainee Trainee { get; set; }

        public int CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]
        public virtual Course Course { get; set; }

        public string Content { get; set; }

    }
}
