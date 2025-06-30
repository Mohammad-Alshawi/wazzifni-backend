using Abp.Application.Services.Dto;

namespace Wazzifni.Domain.MobileApps.Dtos;

public class UpdateMobileAppDto : CreateMobileAppDto, IEntityDto
{
    public int Id { get; set; }
}
