using Abp.Authorization.Users;
using Abp.Extensions;
using System;
using System.Collections.Generic;
using Wazzifni.Domain.Companies;
using Wazzifni.Domain.IndividualUserProfiles;
using Wazzifni.Domain.Trainees;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Authorization.Users
{
    public class User : AbpUser<User>
    {
        public const string DefaultPassword = "123qwe";

        public string DialCode { get; set; }
        public string RegistrationFullName { get; set; }
        public string FcmToken { get; set; }
        public UserType Type { get; set; }
        public string InvitationCode { get; set; }

        public int? CompanyId { get; set; }
        public Company Company { get; set; }

        public long? ProfileId { get; set; }
        public Profile Profile { get; set; }

        public long? TraineeId { get; set; }
        public Trainee Trainee { get; set; }


        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public static User CreateTenantAdminUser(int tenantId, string emailAddress)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                EmailAddress = emailAddress,
                Roles = new List<UserRole>()
            };

            user.SetNormalizedNames();

            return user;
        }
    }
}
