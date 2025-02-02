using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;

namespace Wazzifni.Domain.SpokenLanguages;

public class SpokenLanguage : FullAuditedEntity, IActiveEntity
{
    [MaxLength(8)] public string Name { get; set; }
    [MaxLength(32)] public string DisplayName { get; set; }
    public bool IsActive { get; set; }
}