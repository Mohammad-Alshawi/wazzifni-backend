using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using System;
using System.ComponentModel.DataAnnotations;
using Wazzifni.Authorization.Users;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Users.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserDto : EntityDto<long>
    {
        [Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxSurnameLength)]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        public bool IsActive { get; set; }

        public string FullName { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public DateTime CreationTime { get; set; }

        public string[] RoleNames { get; set; }

        public DateTime? LastActivationTime { get; set; }
        public bool IsClient { get; set; }
        public string DialCode { get; set; }
        public string PhoneNumber { get; set; }
        public UserType Type { get; set; }
        public LiteAttachmentDto ProfilePhoto { get; set; }
        public string RegistrationFullName { get; set; }



    }
}
