using Abp.Application.Services.Dto;

namespace Wazzifni.SpokenLanguages.DTOs
{
    public class LiteSpokenLanguageDto : EntityDto
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public LiteAttachmentDto Icon { get; set; }

    }
}
