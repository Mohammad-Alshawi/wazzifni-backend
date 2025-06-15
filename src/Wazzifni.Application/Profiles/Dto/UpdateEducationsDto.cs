using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace Wazzifni.Profiles.Dto
{
    public class UpdateEducationsDto : IEntityDto<long>
    {
        public List<EducationDto> Educations { get; set; }
        public long Id { get; set; }

    }

}
