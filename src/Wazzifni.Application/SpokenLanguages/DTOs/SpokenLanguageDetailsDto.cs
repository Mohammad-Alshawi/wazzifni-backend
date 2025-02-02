using Abp.Application.Services.Dto;

namespace Wazzifni.SpokenLanguages.DTOs
{
    public class SpokenLanguageDetailsDto : EntityDto
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public LiteAttachmentDto Icon { get; set; }

    }
}
