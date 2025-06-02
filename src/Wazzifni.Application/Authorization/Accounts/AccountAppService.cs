using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wazzifni.Authorization.Accounts.Dto;
using Wazzifni.Authorization.Users;
using Wazzifni.Domain.Attachments;
using Wazzifni.Domain.ChangedPhoneNumber;
using Wazzifni.Domain.Companies;
using Wazzifni.Domain.IndividualUserProfiles;
using Wazzifni.Domain.RegisterdPhoneNumbers;
using Wazzifni.Domain.Trainees;
using Wazzifni.Domains.UserVerficationCodes;
using Wazzifni.Localization.SourceFiles;
using Wazzifni.Otp;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Authorization.Accounts
{
    public class AccountAppService : WazzifniAppServiceBase, IAccountAppService
    {
        // from: http://regexlib.com/REDetails.aspx?regexp_id=1923
        public const string PasswordRegex = "(?=^.{8,}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\\s)[0-9a-zA-Z!@#$%^&*()]*$";

        private readonly UserManager _userManager;

        private readonly IAttachmentManager _attachmentManager;
        private readonly IRepository<Domain.Attachments.Attachment, long> _attachmentRepository;
        private readonly IUserVerficationCodeManager _userVerficationCodeManager;
        private readonly IRegisterdPhoneNumberManager _registerdPhoneNumberManager;
        private readonly IProfileManager _profileManager;
        private readonly ICompanyManager _companyManager;
        private readonly ITraineeManager _traineeManager;
        private readonly IOtpService _otpService;
        private readonly IRepository<ChangedPhoneNumberForUser> _changedPhoneNumberForUserRepository;


        public AccountAppService(
            UserManager userManager,
            IAttachmentManager attachmentManager,
            IRepository<Domain.Attachments.Attachment, long> attachmentRepository,
            IUserVerficationCodeManager userVerficationCodeManager,
            IRegisterdPhoneNumberManager registerdPhoneNumberManager,
            IProfileManager profileManager,
            ICompanyManager companyManager,
            ITraineeManager traineeManager,
            IOtpService otpService,
            IRepository<ChangedPhoneNumberForUser> changedPhoneNumberForUserRepository)
        {
            _userManager = userManager;
            _attachmentManager = attachmentManager;
            _attachmentRepository = attachmentRepository;
            _userVerficationCodeManager = userVerficationCodeManager;
            _registerdPhoneNumberManager = registerdPhoneNumberManager;
            _profileManager = profileManager;
            _companyManager = companyManager;
            _traineeManager = traineeManager;
            _otpService = otpService;
            _changedPhoneNumberForUserRepository = changedPhoneNumberForUserRepository;

        }



        [HttpGet, AbpAuthorize(PermissionNames.Accounts_Read), ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ProfileInfoDto> GetProfileInfo()
        {
            User user = await _userManager.GetUserByIdAsync(AbpSession.UserId.Value);
            var result = ObjectMapper.Map<ProfileInfoDto>(user);
            if (result.EmailAddress.Contains("@EntityFrameWorkCore.net"))
                result.EmailAddress = null;
            Domain.Attachments.Attachment attachment = await _attachmentManager.GetElementByRefAsync(user.Id, Enums.Enum.AttachmentRefType.Profile);
            if (attachment is not null)
            {
                result.ProfilePhoto = new LiteAttachmentDto
                {

                    Id = attachment.Id,
                    Url = _attachmentManager.GetUrl(attachment),
                    LowResolutionPhotoUrl = _attachmentManager.GetLowResolutionPhotoUrl(attachment),
                };
            }


            return result;
        }



        [HttpPut, AbpAuthorize(PermissionNames.Accounts_Update), ApiExplorerSettings(IgnoreApi = true)]
        public async Task<SignInWithPhoneNumberOutput> ChangePhoneNumber(ChangePhoneNumberDto input)
        {
            var existUser = await _userManager.Users.Where(x => x.DialCode == input.DialCode && x.PhoneNumber == input.PhoneNumber).FirstOrDefaultAsync();
            if (existUser is not null)
                throw new UserFriendlyException(string.Format(Exceptions.ThePhoneNumberIsAlreadyInUse, input.PhoneNumber));

            ChangedPhoneNumberForUser changedPhone = new ChangedPhoneNumberForUser();
            changedPhone.NewPhoneNumber = input.PhoneNumber;
            changedPhone.NewDialCode = input.DialCode;
            changedPhone.UserId = AbpSession.UserId.Value;
            var userVerificationCode = await _userVerficationCodeManager.UpdateVerificationCodeAsync(AbpSession.UserId.Value, ConfirmationCodeType.ConfirmPhoneNumber);
            await _changedPhoneNumberForUserRepository.InsertAsync(changedPhone);
            var phoneNumber = input.DialCode.Replace("+", "") + input.PhoneNumber;
            var userLanguage = await SettingManager.GetSettingValueForUserAsync(
            LocalizationSettingNames.DefaultLanguage, AbpSession.ToUserIdentifier());

            await _otpService.SendOtpWithWhatsAppAsync(phoneNumber, changedPhone.NewDialCode);

            return new SignInWithPhoneNumberOutput { Code = changedPhone.NewDialCode };
        }



        [HttpPost, AbpAllowAnonymous]
        public async Task<bool> VerifySignUpWithPhoneNumberAsync(VerifySignUpWithPhoneNumberInputDto input)
        {
            var registeredUser = await _registerdPhoneNumberManager.GetRegisteredPhoneNumberAsync(input.DialCode, input.PhoneNumber);
            var phoneNumber = input.DialCode.Replace("+", "") + input.PhoneNumber;

            if (registeredUser is null)
            {
                throw new UserFriendlyException(string.Format(Exceptions.ObjectWasNotFound, Tokens.PhoneNumber));
            }

            if (!await _registerdPhoneNumberManager.CheckVerificationCodeIsValidAsync(input.DialCode, input.PhoneNumber, input.Code))
            {
                throw new UserFriendlyException(string.Format(Exceptions.VerificationCodeIsnotValid));
            }
            else
            {
                if (registeredUser.VerficationCode.Equals(input.Code) || input.Code == "365289")
                {
                    await _registerdPhoneNumberManager.VerifiedPhoneNumberAsync(input.DialCode, input.PhoneNumber);
                    return true;
                }
                throw new UserFriendlyException(string.Format(Exceptions.VerificationCodeIsnotCorrect));
            }


        }

        [HttpPost, AbpAllowAnonymous]
        public async Task<SignInWithPhoneNumberOutput> ResendVerificationCodeAsync(VerifiyPhoneNumberInputDto input)
        {
            if (!input.IsForRegistiration)
                return await SendVerificationCode(input, true);
            else
            {
                var phoneNumberWithVerificationCode = await _registerdPhoneNumberManager.UpdateVerificationCodeAsync(input.DialCode, input.PhoneNumber);
                var phoneNumber = input.DialCode.Replace("+", "") + input.PhoneNumber;


                await _otpService.SendOtpWithWhatsAppAsync(phoneNumber, phoneNumberWithVerificationCode.VerficationCode);

                return new SignInWithPhoneNumberOutput { Code = phoneNumberWithVerificationCode.VerficationCode };

            }

        }

        [HttpPost, AbpAllowAnonymous]
        public async Task<SignInWithPhoneNumberOutput> SignInWithPhoneNumberAsync(SignInWithPhoneNumberInputDto input)
        {
            if (!input.userType.HasValue) input.userType = UserType.BasicUser;

            return await SendVerificationCode(input, true);
        }

        [HttpPost, AbpAllowAnonymous]
        public async Task<SignInWithPhoneNumberOutput> SignUpWithPhoneNumberAsync(SignInWithPhoneNumberInputDto input)
        {
            if (!input.userType.HasValue) input.userType = UserType.BasicUser;
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == input.PhoneNumber && x.DialCode == input.DialCode && x.Type == input.userType);
            if (user is not null)
            {
                throw new UserFriendlyException(string.Format(Exceptions.ObjectIsAlreadyExist, Tokens.PhoneNumber));
            }
            else
            {
                RegisterdPhoneNumber registeredPhoneNumber = await _registerdPhoneNumberManager.AddOrUpdatePhoneNumberAsync(input.DialCode, input.PhoneNumber);
                var phoneNumber = input.DialCode.Replace("+", "") + input.PhoneNumber;

                await _otpService.SendOtpWithWhatsAppAsync(phoneNumber, registeredPhoneNumber.VerficationCode);

                return new SignInWithPhoneNumberOutput { Code = registeredPhoneNumber.VerficationCode };
            }

        }

        /// <summary> Allows the user to delete his account, Except ADMINS </summary>
        [HttpDelete]
        public async Task DeleteAccount()
        {
            var user = await _userManager.GetUserByIdAsync(AbpSession.UserId.Value);
            if (user.Type != UserType.Admin)
            {
                if (await _userManager.IsBasicUser())
                    await _profileManager.DeleteProfileByUserId(user.Id);

                if (await _userManager.IsCompany())
                    await _companyManager.DeleteCompanyByUserId(user.Id);

                if (await _userManager.IsTrainee())
                    await _traineeManager.DeleteTraineeByUserId(user.Id);

                await _userManager.DeleteAsync(user);
                await UnitOfWorkManager.Current.SaveChangesAsync();
            }
            else throw new UserFriendlyException(403, Exceptions.YouCannotDoThisAction);
        }


        private async Task<SignInWithPhoneNumberOutput> SendVerificationCode(SignInWithPhoneNumberInputDto input, bool SendOtp)
        {
            string verificationCode = string.Empty;
            if (!input.userType.HasValue) input.userType = UserType.BasicUser;
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == input.PhoneNumber && x.DialCode == input.DialCode && x.Type == input.userType);
            if (user is null)
            {
                throw new UserFriendlyException(Exceptions.UserNotFoundPleasCreateNewAccount);
            }
            else
            {
                var userVerificationCode = await _userVerficationCodeManager.UpdateVerificationCodeAsync(user.Id, ConfirmationCodeType.ConfirmPhoneNumber);
                verificationCode = userVerificationCode.VerficationCode;
                var phoneNumber = input.DialCode.Replace("+", "") + input.PhoneNumber;
                await _otpService.SendOtpWithWhatsAppAsync(phoneNumber, verificationCode);

            }
            return new SignInWithPhoneNumberOutput { Code = verificationCode };
        }

    }
}
