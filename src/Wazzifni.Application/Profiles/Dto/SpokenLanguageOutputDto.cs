using Wazzifni.SpokenLanguages.DTOs;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Profiles.Dto
{
    public class SpokenLanguageOutputDto
    {
        public SpokenLanguageDto SpokenLanguage { get; set; }

        public SpokenLanguageLevel OralLevel { get; set; }

        public SpokenLanguageLevel WritingLevel { get; set; }

        public bool IsNative { get; set; }
    }
}
