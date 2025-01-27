using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Domains.UserVerficationCodes
{
    public class UserVerficationCodeManager : DomainService, IUserVerficationCodeManager
    {
        private readonly IRepository<UserVerficationCode, long> _repositoryUserVerficationCode;
        public UserVerficationCodeManager(IRepository<UserVerficationCode, long> repositoryUserVerficationCode)
        {
            _repositoryUserVerficationCode = repositoryUserVerficationCode;
        }

        public async Task<UserVerficationCode> AddUserVerficationCodeAsync(UserVerficationCode userKey)
        {
            await _repositoryUserVerficationCode.InsertAsync(userKey);
            await UnitOfWorkManager.Current.SaveChangesAsync();
            return userKey;
        }

        public async Task<UserVerficationCode> UpdateVerificationCodeAsync(long UserId, ConfirmationCodeType confirmationCodeType)
        {
            Random generator = new Random();
            string randomNumber = generator.Next(0, 1000000).ToString("D6");
            var userHasCode = await _repositoryUserVerficationCode.GetAll().FirstOrDefaultAsync(x => x.UserId == UserId && x.ConfirmationCodeType == confirmationCodeType);
            if (userHasCode is not null)
            {
                userHasCode.VerficationCode = randomNumber;
                await _repositoryUserVerficationCode.UpdateAsync(userHasCode);
            }
            else
            {
                userHasCode = await AddUserVerficationCodeAsync(new UserVerficationCode
                {
                    UserId = UserId,
                    VerficationCode = randomNumber,
                    ConfirmationCodeType = confirmationCodeType

                });
            }
            return userHasCode;

        }
        public async Task<UserVerficationCode> GetUserWithVerificationCodeAsync(long UserId, ConfirmationCodeType confirmationCodeType)
        {
            return await _repositoryUserVerficationCode.GetAll().FirstOrDefaultAsync(x => x.UserId == UserId && x.ConfirmationCodeType == confirmationCodeType);
        }

        public async Task<string> GetUserVerificationCodeAsync(long UserId)
        {
            var userVerificationCode = await _repositoryUserVerficationCode.GetAll().FirstOrDefaultAsync(x => x.UserId == UserId);
            return userVerificationCode.VerficationCode;
        }

        public async Task<bool> CheckVerificationCodeIsValidAsync(long UserId, ConfirmationCodeType confirmationCodeType)
        {
            var code = await _repositoryUserVerficationCode.GetAll().FirstOrDefaultAsync(x => x.UserId == UserId && x.ConfirmationCodeType == confirmationCodeType);
            if (code.CreationTime.ToUniversalTime().AddMinutes(WazzifniConsts.VerificationTimeCodeInMinutes) > DateTime.UtcNow && !code.LastModificationTime.HasValue)
            {
                return true;
            }
            else if (code.LastModificationTime.HasValue && code.LastModificationTime.Value.ToUniversalTime() > code.CreationTime.ToUniversalTime() && code.LastModificationTime?.ToUniversalTime().AddMinutes(WazzifniConsts.VerificationTimeCodeInMinutes) > DateTime.UtcNow)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task ClearCodeAfterVerify(long entityId)
        {
            var entity = await _repositoryUserVerficationCode.GetAsync(entityId);
            entity.VerficationCode = null;
            await _repositoryUserVerficationCode.UpdateAsync(entity);
            await UnitOfWorkManager.Current.SaveChangesAsync();
        }
    }
}
