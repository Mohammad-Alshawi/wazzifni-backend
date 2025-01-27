using Abp.Authorization.Users;
using System.ComponentModel.DataAnnotations;

namespace Wazzifni.Authorization.Accounts.Dto
{
    public class UpdateProfileDto
    {
        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }

        //[Required]
        //[StringLength(AbpUserBase.MaxSurnameLength)]
        //public string Surname { get; set; }
        //[Required]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }
        [Required]
        public long ProfilePhoto { get; set; }
    }
}
