using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Wazzifni.Countries;
using Wazzifni.Domain.Regions;

namespace Wazzifni.Domain.Cities
{
    //city model
    public class City : FullAuditedEntity, IActiveEntity, IMultiLingualEntity<CityTranslation>
    {
        public City()
        {
            Regions = new HashSet<Region>();
            Translations = new HashSet<CityTranslation>();
        }

        public int CountryId { get; set; }
        [ForeignKey(nameof(CountryId))]
        public virtual Country Country { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<Region> Regions { get; set; }
        public ICollection<CityTranslation> Translations { get; set; }
    }
}
