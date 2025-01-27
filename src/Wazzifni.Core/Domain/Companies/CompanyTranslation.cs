using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Wazzifni.Domain.Companies
{
    public class CompanyTranslation : FullAuditedEntity, IEntityTranslation<Company>
    {
        public string Name { get; set; }
        public string About { get; set; }
        public string Address { get; set; }
        public Company Core { get; set; }
        public int CoreId { get; set; }
        public string Language { get; set; }
    }
}
