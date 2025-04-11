using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Wazzifni.Domain.Universities;

namespace Wazzifni.Domain.CourseTags
{
    public class CourseTag : FullAuditedEntity, IActiveEntity, IMultiLingualEntity<CourseTagTranslation>
    {
        public bool IsActive { get; set; }
        public ICollection<CourseTagTranslation> Translations { get; set; }
    }
   
}
