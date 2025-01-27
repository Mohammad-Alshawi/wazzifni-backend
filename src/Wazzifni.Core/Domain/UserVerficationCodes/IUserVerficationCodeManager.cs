using Abp.Domain.Services;
using System.Threading.Tasks;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Domains.UserVerficationCodes
{
    public interface IUserVerficationCodeManager : IDomainService
    {
        Task<UserVerficationCode> AddUserVerficationCodeAsync(UserVerficationCode userKey);

        Task<UserVerficationCode> UpdateVerificationCodeAsync(long UserId, ConfirmationCodeType confirmationCodeType);

        Task<UserVerficationCode> GetUserWithVerificationCodeAsync(long UserId, ConfirmationCodeType confirmationCodeType);

        Task<string> GetUserVerificationCodeAsync(long UserId);
        Task<bool> CheckVerificationCodeIsValidAsync(long UserId, ConfirmationCodeType confirmationCodeType);
        Task ClearCodeAfterVerify(long entityId);

    }
}
