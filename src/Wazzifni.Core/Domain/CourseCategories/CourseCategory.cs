using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Wazzifni.Domain.Universities;

namespace Wazzifni.Domain.CourseCategories
{
    public class CourseCategory : FullAuditedEntity, IActiveEntity, IMultiLingualEntity<CourseCategoryTranslation>
    {
        public bool IsActive { get; set; }
        public ICollection<CourseCategoryTranslation> Translations { get; set; }
    }
   
}
