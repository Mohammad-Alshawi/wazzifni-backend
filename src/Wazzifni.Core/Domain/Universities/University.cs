using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Wazzifni.Domain.Companies;

namespace Wazzifni.Domain.Universities
{
    public class University : FullAuditedEntity, IActiveEntity, IMultiLingualEntity<UniversityTranslation>
    {
        public bool IsActive { get ; set; }
        public ICollection<UniversityTranslation> Translations { get; set; }
    }
}
