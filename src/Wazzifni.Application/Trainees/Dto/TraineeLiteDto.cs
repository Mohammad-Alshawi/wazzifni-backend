using Abp.Application.Services.Dto;
using Wazzifni.Cities.Dto;
using Wazzifni.Companies.Dto;
using Wazzifni.Universities.Dto;

namespace Wazzifni.Trainees.Dto
{
    public class TraineeLiteDto : EntityDto<long>
    {
        public string? UniversityMajor { get; set; }
        public LiteUniversityDto University { get; set; }
        public SuperLiteUserDto User { get; set; }

        public LiteAttachmentDto Image { get; set; }

    }
}
