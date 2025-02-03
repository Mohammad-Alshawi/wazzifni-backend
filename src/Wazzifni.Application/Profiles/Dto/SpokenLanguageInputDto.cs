using static Wazzifni.Enums.Enum;

namespace Wazzifni.Profiles.Dto
{
    public class SpokenLanguageInputDto
    {
        public int SpokenLanguageId { get; set; }

        public SpokenLanguageLevel OralLevel { get; set; }

        public SpokenLanguageLevel WritingLevel { get; set; }

        public bool IsNative { get; set; }
    }
}
