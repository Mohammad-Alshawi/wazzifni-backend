using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Wazzifni.Domain.Universities;
using Wazzifni.Domain.CourseTags;

namespace Wazzifni.Domain.CourseTags
{
    public class CourseTagTranslation : FullAuditedEntity, IEntityTranslation<CourseTag>
    {
        public string Name { get; set; }
        public CourseTag Core { get; set; }
        public int CoreId { get; set; }
        public string Language { get; set; }
    }
   
}
