using System.ComponentModel.DataAnnotations;

namespace Wazzifni.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}