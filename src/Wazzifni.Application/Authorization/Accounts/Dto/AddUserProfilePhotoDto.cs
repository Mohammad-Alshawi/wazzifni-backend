using System.ComponentModel.DataAnnotations;

namespace Wazzifni.Authorization.Accounts.Dto
{
    public class AddUserProfilePhotoDto
    {
        [Required]
        public long PhotoId { get; set; }
    }
}

