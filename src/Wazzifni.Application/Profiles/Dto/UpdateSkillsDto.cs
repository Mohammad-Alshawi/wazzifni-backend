using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace Wazzifni.Profiles.Dto
{
    public class UpdateSkillsDto : IEntityDto<long>
    {
        public List<int> SkillIds { get; set; }
        public long Id { get; set; }
    }

}
