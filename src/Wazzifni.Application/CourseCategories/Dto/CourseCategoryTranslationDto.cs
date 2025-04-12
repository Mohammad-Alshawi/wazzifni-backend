using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using Wazzifni.Domain.Cities;
using Wazzifni.Domain.CourseCategories;

namespace Wazzifni.CourseCategories.Dto

{
    /// <summary>
    /// Post Category Translation Dto
    /// </summary>
    [AutoMap(typeof(CourseCategoryTranslation))]
    public class CourseCategoryTranslationDto
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
