using System.Collections.Generic;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Profiles.Dto
{
    public class CreateProfileDto
    {
        public string About { get; set; }

        public int CityId { get; set; }

        public List<EducationDto> Educations { get; set; } = new List<EducationDto>();

        public List<WorkExperienceDto> WorkExperiences { get; set; } = new List<WorkExperienceDto>();

        public List<AwardDto> Awards { get; set; } = new List<AwardDto>();

        public List<SpokenLanguageInputDto> SpokenLanguages { get; set; } = new List<SpokenLanguageInputDto>();
        public List<int> SkillIds { get; set; }

        public long? ProfilePhotoId { get; set; }
        public long CvId { get; set; }

        public Gender? Gender { get; set; }
    }
}
