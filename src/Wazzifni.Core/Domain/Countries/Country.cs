using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Wazzifni.Domain;
using Wazzifni.Domain.Cities;
using Wazzifni.Domain.Countries;

namespace Wazzifni.Countries
{
    public class Country : FullAuditedEntity, IActiveEntity, IMultiLingualEntity<CountryTranslation>
    {
        public Country()
        {

            Cities = new HashSet<City>();
            Translations = new HashSet<CountryTranslation>();
        }

        public bool IsActive { get; set; }
        [Required]
        [StringLength(5)]
        public string DialCode { get; set; }
        public virtual ICollection<City> Cities { get; set; }
        public ICollection<CountryTranslation> Translations { get; set; }
    }
}
