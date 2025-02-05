using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Wazzifni.Authorization.Users;
using Wazzifni.Domain.Cities;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Domain.Companies
{
    public class Company : FullAuditedEntity, IActiveEntity, IMultiLingualEntity<CompanyTranslation>
    {

        // public int? CompanyContactId { get; set; }
        // [ForeignKey(nameof(CompanyContactId))]
        // public virtual CompanyContact CompanyContact { get; set; }

        public int? CityId { get; set; }
        [ForeignKey(nameof(CityId))]
        public virtual City City { get; set; }
        public long? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
        public virtual ICollection<CompanyTranslation> Translations { get; set; }
        public bool IsActive { get; set; }
        public CompanyStatus Status { get; set; }

        public string JobType { get; set; }
        public DateTime? DateOfEstablishment { get; set; }
        public string ReasonRefuse { get; set; }
        public int? NumberOfEmployees { get; set; }
        public string WebSite { get; set; }

    }
}
