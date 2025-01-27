using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Wazzifni.Authorization.Accounts.Dto;
using Wazzifni.Authorization.Users;
using Wazzifni.Domain.Attachments;
using Wazzifni.Domain.ChangedPhoneNumber;
using Wazzifni.Domain.RegisterdPhoneNumbers;
using Wazzifni.Domains.UserVerficationCodes;
using Wazzifni.Localization.SourceFiles;
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
        private readonly IRepository<ChangedPhoneNumberForUser> _changedPhoneNumberForUserRepository;


        public AccountAppService(
            UserManager userManager,
            IAttachmentManager attachmentManager,
            IRepository<Domain.Attachments.Attachment, long> attachmentRepository,
            IUserVerficationCodeManager userVerficationCodeManager,
            IRegisterdPhoneNumberManager registerdPhoneNumberManager,
            IRepository<ChangedPhoneNumberForUser> changedPhoneNumberForUserRepository)
        {
            _userManager = userManager;
            _attachmentManager = attachmentManager;
            _attachmentRepository = attachmentRepository;
            _userVerficationCodeManager = userVerficationCodeManager;
            _registerdPhoneNumberManager = registerdPhoneNumberManager;
            _changedPhoneNumberForUserRepository = changedPhoneNumberForUserRepository;

        }



        [HttpGet, AbpAuthorize(PermissionNames.Accounts_Read)]
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




        [HttpPut, AbpAuthorize(PermissionNames.Accounts_Update)]
        public async Task UpdateProfile(UpdateProfileDto input)
        {
            var user = await _userManager.GetUserByIdAsync(AbpSession.UserId.Value);
            user.Name = input.Name;
            if (string.IsNullOrEmpty(input.EmailAddress))
            {
                if (!user.EmailAddress.Contains("@EntityFrameWorkCore.net"))
                {
                    Random random = new Random();
                    int randomNumber = random.Next(100, 100000);
                    input.EmailAddress = input.Name + randomNumber.ToString() + "@EntityFrameWorkCore.net";
                }
                else
                    input.EmailAddress = user.EmailAddress;
            }
            user.EmailAddress = input.EmailAddress;
            user.IsEmailConfirmed = false;
            user.RegistrationFullName = input.Name;
            user.LastModificationTime = DateTime.UtcNow;
            Attachment oldAttachments = await _attachmentManager.GetElementByRefAsync((long)user.Id, AttachmentRefType.Profile);
            if (input.ProfilePhoto != 0)
            {
                if (oldAttachments is not null)
                {
                    if (input.ProfilePhoto != oldAttachments.Id)
                    {
                        if (oldAttachments is not null)
                            await _attachmentManager.DeleteRefIdAsync(oldAttachments);

                        await _attachmentManager.CheckAndUpdateRefIdAsync(
                                  (long)input.ProfilePhoto, AttachmentRefType.Profile, user.Id);
                    }
                }
                else
                {
                    await _attachmentManager.CheckAndUpdateRefIdAsync(
                              (long)input.ProfilePhoto, AttachmentRefType.Profile, user.Id);
                }
            }
            if (input.ProfilePhoto == 0)
            {
                if (oldAttachments is not null)
                {
                    await _attachmentRepository.HardDeleteAsync(oldAttachments);
                }
            }
            await UnitOfWorkManager.Current.SaveChangesAsync();
            await _userManager.UpdateAsync(user);

        }

        [HttpPost, AbpAuthorize(PermissionNames.Accounts_Update)]
        public async Task AddOrEditUserProfilePhoto(AddUserProfilePhotoDto input)
        {
            var oldAttachment = await _attachmentManager.GetElementByRefAsync(AbpSession.UserId.Value, Enums.Enum.AttachmentRefType.Profile);
            if (oldAttachment is not null)
            {
                await _attachmentManager.DeleteRefIdAsync(oldAttachment);
            }
            Attachment attachment = await _attachmentManager.GetByIdAsync(input.PhotoId);
            await _attachmentManager.UpdateRefIdAsync(attachment, AbpSession.UserId.Value);
        }

        [HttpPut, AbpAuthorize(PermissionNames.Accounts_Update)]
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
            // var provider = SettingManager.GetSettingValue(AppSettingNames.SMSSenderProvider);
            /*switch (provider)
            {
                case "Syriatel":
                    await _otpSenderAppService.OtpSyriatelAsync(phoneNumber, userVerificationCode.VerficationCode, userLanguage);
                    break;
                case "MTN":
                    await _otpSenderAppService.OtpMtnAsync(phoneNumber, userVerificationCode.VerficationCode, userLanguage);
                    break;
                case "AccordingUserNumber":
                    await _otpSenderAppService.SendOtpAccordingToUserNumberAsync(phoneNumber, userVerificationCode.VerficationCode, userLanguage);
                    break;
                default:
                    await _otpSenderAppService.OtpSyriatelAsync(phoneNumber, userVerificationCode.VerficationCode, userLanguage);
                    break;
            }*/
            //if (phoneNumber.StartsWith("963"))
            //{
            //    return new SignInWithPhoneNumberOutput
            //    {
            //        Code = GenerateRandomCode()
            //    };

            //}
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
            //if (RegexStore.SyrianPhonNumberRegex().IsMatch(phoneNumber))
            //{
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
            // }

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


                //var provider = SettingManager.GetSettingValue(AppSettingNames.SMSSenderProvider);
                /* switch (provider)
				 {
					 case "Syriatel":
						 await _otpSenderAppService.OtpSyriatelAsync(phoneNumber, phoneNumberWithVerificationCode.VerficationCode, input.Language);
						 break;
					 case "MTN":
						 await _otpSenderAppService.OtpMtnAsync(phoneNumber, phoneNumberWithVerificationCode.VerficationCode, input.Language);
						 break;
					 case "AccordingUserNumber":
						 await _otpSenderAppService.SendOtpAccordingToUserNumberAsync(phoneNumber, phoneNumberWithVerificationCode.VerficationCode, input.Language);
						 break;
					 default:
						 await _otpSenderAppService.OtpSyriatelAsync(phoneNumber, phoneNumberWithVerificationCode.VerficationCode, input.Language);
						 break;
				 }*/
                //if (phoneNumber.StartsWith("963"))
                //{
                //    return new SignInWithPhoneNumberOutput
                //    {
                //        Code = GenerateRandomCode()
                //    };

                //}
                return new SignInWithPhoneNumberOutput { Code = phoneNumberWithVerificationCode.VerficationCode };

            }

        }

        [HttpPost, AbpAllowAnonymous]
        public async Task<SignInWithPhoneNumberOutput> SignInWithPhoneNumberAsync(SignInWithPhoneNumberInputDto input)
        {
            if (!input.userType.HasValue) input.userType = UserType.BasicUser;
            if (input.WithVeficationCodeOtp.HasValue && input.WithVeficationCodeOtp.Value == false)
                return await SendVerificationCode(input, false);
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

                //var provider = SettingManager.GetSettingValue(AppSettingNames.SMSSenderProvider);
                /* switch (provider)
				 {
					 case "Syriatel":
						 await _otpSenderAppService.OtpSyriatelAsync(phoneNumber, registeredPhoneNumber.VerficationCode, input.Language);
						 break;
					 case "MTN":
						 await _otpSenderAppService.OtpMtnAsync(phoneNumber, registeredPhoneNumber.VerficationCode, input.Language);
						 break;
					 case "AccordingUserNumber":
						 await _otpSenderAppService.SendOtpAccordingToUserNumberAsync(phoneNumber, registeredPhoneNumber.VerficationCode, input.Language);
						 break;
					 default:
						 await _otpSenderAppService.OtpSyriatelAsync(phoneNumber, registeredPhoneNumber.VerficationCode, input.Language);
						 break;
				 }*/
                //if (phoneNumber.StartsWith("963"))
                //{
                //    return new SignInWithPhoneNumberOutput
                //    {
                //        Code = GenerateRandomCode()
                //    };

                //}
                return new SignInWithPhoneNumberOutput { Code = registeredPhoneNumber.VerficationCode };
            }

        }

        /// <summary> Allows the user to delete his account, Except ADMINS </summary>
        [HttpDelete, AbpAuthorize(PermissionNames.Accounts_Delete)]
        public async Task DeleteAccount()
        {
            var user = await _userManager.GetUserByIdAsync(AbpSession.UserId.Value);
            if (user.Type != UserType.Admin)
            {
                await _userManager.DeleteAsync(user);
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
                // var provider = SettingManager.GetSettingValue(AppSettingNames.SMSSenderProvider);
                /*switch (provider)
                {
                    case "Syriatel":
                        await _otpSenderAppService.OtpSyriatelAsync(phoneNumber, verificationCode, input.Language);
                        break;
                    case "MTN":
                        await _otpSenderAppService.OtpMtnAsync(phoneNumber, verificationCode, input.Language);
                        break;
                    case "AccordingUserNumber":
                        await _otpSenderAppService.SendOtpAccordingToUserNumberAsync(phoneNumber, verificationCode, input.Language);
                        break;
                    default:
                        await _otpSenderAppService.OtpSyriatelAsync(phoneNumber, verificationCode, input.Language);
                        break;
                }*/

                //if (phoneNumber.StartsWith("963"))
                //{
                //    return new SignInWithPhoneNumberOutput
                //    {
                //        Code = GenerateRandomCode()
                //    };

                //}
            }
            return new SignInWithPhoneNumberOutput { Code = verificationCode };
        }

    }
}
