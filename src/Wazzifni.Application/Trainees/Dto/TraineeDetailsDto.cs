using Abp.Application.Services.Dto;
using System.Collections.Generic;
using Wazzifni.Cities.Dto;
using Wazzifni.Companies.Dto;
using Wazzifni.Skills.Dto;
using Wazzifni.Universities.Dto;

namespace Wazzifni.Trainees.Dto
{
    public class TraineeDetailsDto : EntityDto<long>
    {
        public string? UniversityMajor { get; set; }
        public LiteUniversityDto University { get; set; }
        public SuperLiteUserDto User { get; set; }

        public LiteAttachmentDto Image { get; set; }

    }
}
