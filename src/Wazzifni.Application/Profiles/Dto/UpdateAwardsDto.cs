using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace Wazzifni.Profiles.Dto
{
    public class UpdateAwardsDto : IEntityDto<long>
    {
        public List<AwardDto> Awards { get; set; }
        public long Id { get; set; }

    }

}
