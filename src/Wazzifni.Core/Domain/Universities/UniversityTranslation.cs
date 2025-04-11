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
    public class UniversityTranslation : FullAuditedEntity, IEntityTranslation<University>
    {
        public string Name { get; set; }
        public University Core { get; set; }
        public int CoreId { get; set; }
        public string Language { get; set; }
    }
}
