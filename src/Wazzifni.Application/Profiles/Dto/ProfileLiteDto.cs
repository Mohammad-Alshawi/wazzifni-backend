using Abp.Application.Services.Dto;
using Wazzifni.Cities.Dto;
using Wazzifni.Companies.Dto;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Profiles.Dto
{
    public class ProfileLiteDto : EntityDto<long>
    {
        public string About { get; set; }
        public LiteCity City { get; set; }
        public SuperLiteUserDto User { get; set; }

        public LiteAttachmentDto Image { get; set; }

        public Gender? Gender { get; set; }

    }
}
