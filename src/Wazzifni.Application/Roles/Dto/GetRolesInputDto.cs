using Abp.Runtime.Validation;
using Abp.UI;
using KeyFinder;
using System.ComponentModel.DataAnnotations;

namespace Wazzifni.Roles.Dto
{
    public class GetRolesInputDto : ICustomValidate
    {
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string DialCode { get; set; }
        public void AddValidationErrors(CustomValidationContext context)
        {
            if (!RegexStore.DialCodeRegex().IsMatch(DialCode))
            {
                throw new UserFriendlyException("WrongPhoneNumberOrDialCode");
            }
            if (!RegexStore.SignInLogInPhoneNumberRegex().IsMatch(PhoneNumber))
            {
                throw new UserFriendlyException("WrongPhoneNumberOrDialCode");
            }
        }

    }
}
