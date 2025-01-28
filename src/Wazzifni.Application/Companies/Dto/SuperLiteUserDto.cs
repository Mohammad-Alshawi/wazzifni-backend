using Abp.Application.Services.Dto;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Companies.Dto
{
    public class SuperLiteUserDto : EntityDto<long>
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsActive { get; set; }
        public string FullName { get; set; }
        public string DialCode { get; set; }
        public string PhoneNumber { get; set; }
        public UserType Type { get; set; }
    }
}
