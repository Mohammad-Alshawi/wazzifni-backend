using System.ComponentModel.DataAnnotations;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Authorization.Accounts.Dto
{
    public class SignInWithPhoneNumberInputDto
    {
        [Required]
        public string DialCode { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
        public string Language { get; set; }

        public UserType? userType { get; set; }



    }

    public class VerifySignUpWithPhoneNumberInputDto : SignInWithPhoneNumberInputDto
    {
        [Required]
        public string Code { get; set; }
    }



    public class VerifiyPhoneNumberInputDto : SignInWithPhoneNumberInputDto
    {
        [Required]
        public bool IsForRegistiration { get; set; }
    }



}
