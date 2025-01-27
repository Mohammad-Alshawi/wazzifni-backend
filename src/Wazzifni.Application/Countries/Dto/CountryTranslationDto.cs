using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;
using Wazzifni.Domain.Countries;


namespace Wazzifni.Countries.Dto
{
    /// <summary>
    /// Post Category Translation Dto
    /// </summary>
    [AutoMap(typeof(CountryTranslation))]
    public class CountryTranslationDto
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
