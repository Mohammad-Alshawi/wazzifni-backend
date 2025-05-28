using Abp.Application.Services.Dto;
using Wazzifni.Profiles.Dto;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Companies.Dto
{
    public class SuperLiteUserDto : EntityDto<long>
    {
        public string RegistrationFullName { get; set; }

        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsActive { get; set; }
        public string FullName { get; set; }
        public string DialCode { get; set; }
        public string PhoneNumber { get; set; }
        public UserType Type { get; set; }

        public int? CompanyId { get; set; }
        public long? ProfileId { get; set; }

        public string EmailAddress { get; set; }

        public UserProfileLiteDto Profile { get; set; }
        public UserLiteCompanyDto Company { get; set; }


    }
}
