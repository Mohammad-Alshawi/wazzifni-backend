using Abp.Application.Services.Dto;

namespace Wazzifni.SpokenLanguages.DTOs;

public class SpokenLanguageDto : EntityDto
{

    public virtual string Name { get; set; }
    public virtual string DisplayName { get; set; }
}
