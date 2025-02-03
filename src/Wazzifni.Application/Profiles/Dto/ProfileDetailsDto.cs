using Abp.Application.Services.Dto;
using System.Collections.Generic;
using Wazzifni.Cities.Dto;
using Wazzifni.Companies.Dto;

namespace Wazzifni.Profiles.Dto
{
    public class ProfileDetailsDto : EntityDto<long>
    {
        public LiteCity City { get; set; }
        public SuperLiteUserDto User { get; set; }
        public List<EducationDto> Educations { get; set; }

        public List<WorkExperienceDto> WorkExperiences { get; set; }

        public List<AwardDto> Awards { get; set; }

        public List<SpokenLanguageOutputDto> SpokenLanguages { get; set; }
    }
}
