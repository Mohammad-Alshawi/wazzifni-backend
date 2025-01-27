using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;
using Wazzifni.Authorization.Users;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Domains.UserVerficationCodes
{

    public class UserVerficationCode : FullAuditedEntity<long>
    {
        public long UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
        public string VerficationCode { get; set; }
        public ConfirmationCodeType ConfirmationCodeType { get; set; }

        public static bool IsValidConfirmationCodeType(byte confirmationCodeType)
        {
            return System.Enum.IsDefined(typeof(ConfirmationCodeType), confirmationCodeType);
        }

    }
}
