using Abp.Application.Services.Dto;
using System.Collections.Generic;
using Wazzifni.Cities.Dto;
using Wazzifni.Companies.Dto;
using Wazzifni.Skills.Dto;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Profiles.Dto
{
    public class ProfileDetailsDto : EntityDto<long>
    {
        public string About { get; set; }
        public LiteCity City { get; set; }
        public SuperLiteUserDto User { get; set; }
        public List<EducationDto> Educations { get; set; }

        public List<WorkExperienceDto> WorkExperiences { get; set; }

        public List<AwardDto> Awards { get; set; }

        public List<SpokenLanguageOutputDto> SpokenLanguages { get; set; }

        public List<LiteSkillDto> Skills { get; set; }

        public LiteAttachmentDto Image { get; set; }

        public LiteAttachmentDto Cv { get; set; }

        public Gender? Gender { get; set; }

    }
}
