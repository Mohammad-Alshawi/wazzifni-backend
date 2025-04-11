using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using Wazzifni.Domain.Universities;

namespace Wazzifni.Universities.Dto

{
    /// <summary>
    /// Post Category Translation Dto
    /// </summary>
    [AutoMap(typeof(UniversityTranslation))]
    public class UniversityTranslationDto
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// Language
        /// </summary>
        [Required]
        public string Language { get; set; }
    }
}
