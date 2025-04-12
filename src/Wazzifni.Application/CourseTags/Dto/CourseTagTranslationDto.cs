using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using Wazzifni.Domain.CourseTags;

namespace Wazzifni.CourseTags.Dto

{
    /// <summary>
    /// Post Category Translation Dto
    /// </summary>
    [AutoMap(typeof(CourseTagTranslation))]
    public class CourseTagTranslationDto
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
