using Abp.Application.Services.Dto;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Profiles.Dto
{
    public class UpdateBasicProfileDto : IEntityDto<long>
    {
        public long Id { get; set; }

        public Gender? Gender { get; set; }

        public string RegistrationFullName { get; set; }

        public int CityId { get; set; }

    }
}
