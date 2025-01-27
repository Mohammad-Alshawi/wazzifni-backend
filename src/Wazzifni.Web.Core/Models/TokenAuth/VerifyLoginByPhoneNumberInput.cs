using Abp.Runtime.Validation;
using System.ComponentModel.DataAnnotations;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Models.TokenAuth
{
    public class VerifyLoginByPhoneNumberInput
    {
        [Required]
        public string DialCode { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Code { get; set; }

        public UserType? UserType { get; set; }

    }

    public class VerifySignUpByPhoneNumberInput : ICustomValidate
    {
        [Required]
        public string FullName { get; set; }

        public string Email { get; set; }
        [Required]
        public string DialCode { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public UserType? UserType { get; set; }

        public int? CityId { get; set; }

        public void AddValidationErrors(CustomValidationContext context)
        {
            if (UserType.HasValue && UserType == Enums.Enum.UserType.BasicUser && !CityId.HasValue)
                context.Results.Add(new ValidationResult("CityId is required"));
        }
    }
}
