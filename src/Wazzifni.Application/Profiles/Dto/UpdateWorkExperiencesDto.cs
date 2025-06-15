using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace Wazzifni.Profiles.Dto
{
    public class UpdateWorkExperiencesDto : IEntityDto<long>
    {
        public List<WorkExperienceDto> WorkExperiences { get; set; }
        public long Id { get; set; }

    }

}
