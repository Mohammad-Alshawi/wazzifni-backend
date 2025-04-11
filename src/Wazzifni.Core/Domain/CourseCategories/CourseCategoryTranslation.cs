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
    public class CourseCategoryTranslation : FullAuditedEntity, IEntityTranslation<CourseCategory>
    {
        public string Name { get; set; }
        public CourseCategory Core { get; set; }
        public int CoreId { get; set; }
        public string Language { get; set; }
    }
   
}
