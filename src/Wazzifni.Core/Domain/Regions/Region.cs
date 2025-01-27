using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Wazzifni.Domain.Cities;

namespace Wazzifni.Domain.Regions
{
    public class Region : FullAuditedEntity, IActiveEntity, IMultiLingualEntity<RegionTranslation>
    {
        public Region()
        {
            Translations = new HashSet<RegionTranslation>();
        }
        public bool IsActive { get; set; }
        public int CityId { get; set; }
        [ForeignKey(nameof(CityId))]
        public virtual City City { get; set; }
        public ICollection<RegionTranslation> Translations { get; set; }




    }
}
