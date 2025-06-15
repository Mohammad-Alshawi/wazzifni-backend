using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace Wazzifni.Profiles.Dto
{
    public class UpdateSpokenLanguagesDto : IEntityDto<long>
    {
        public List<SpokenLanguageInputDto> SpokenLanguages { get; set; }
        public long Id { get; set; }

    }
}
