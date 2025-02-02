using Abp.Application.Services.Dto;

namespace Wazzifni.SpokenLanguages.DTOs;

public class UpdateSpokenLanguageDto : CreateSpokenLanguageDto, IEntityDto
{
    public int Id { get; set; }
}