using System.ComponentModel.DataAnnotations;

namespace Wazzifni.Advertisiments.Dto
{
    public class AddadvertisimentElementDto : CreateAdvertisimentElementDto
    {
        [Required]
        public int AdvertisimentId { get; set; }
    }
}
