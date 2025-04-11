using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Wazzifni.Universities.Dto;

namespace Wazzifni.Universities.Dto
{
    public class LiteUniversityDto : EntityDto
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationTime { get; set; }
        public List<UniversityTranslationDto> Translations { get; set; }
    }
}
