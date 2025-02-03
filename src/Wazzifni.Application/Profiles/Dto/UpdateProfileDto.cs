using Abp.Application.Services.Dto;

namespace Wazzifni.Profiles.Dto
{
    public class UpdateProfileDto : CreateProfileDto, IEntityDto<long>
    {
        public long Id { get; set; }

    }
}
