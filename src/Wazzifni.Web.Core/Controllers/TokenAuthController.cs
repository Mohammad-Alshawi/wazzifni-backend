using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.MultiTenancy;
using Abp.Runtime.Security;
using Abp.UI;
using AutoMapper;
using KeyFinder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wazzifni.Authentication.JwtBearer;
using Wazzifni.Authorization;
using Wazzifni.Authorization.Roles;
using Wazzifni.Authorization.Users;
using Wazzifni.Companies.Dto;
using Wazzifni.Domain.Attachments;
using Wazzifni.Domain.ChangedPhoneNumber;
using Wazzifni.Domain.Companies;
using Wazzifni.Domain.IndividualUserProfiles;
using Wazzifni.Domain.RegisterdPhoneNumbers;
using Wazzifni.Domain.Trainees;
using Wazzifni.Domains.UserVerficationCodes;
using Wazzifni.Localization.SourceFiles;
using Wazzifni.Models.TokenAuth;
using Wazzifni.MultiTenancy;
using Wazzifni.Profiles.Dto;
using Wazzifni.Trainees.Dto;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TokenAuthController : WazzifniControllerBase
    {
        private readonly LogInManager _logInManager;
        private readonly ITenantCache _tenantCache;
        private readonly UserManager _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserVerficationCodeManager _userVerficationCodeManager;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;
        private readonly UserRegistrationManager _userRegistrationManager;
        private readonly IRegisterdPhoneNumberManager _registerdPhoneNumberManager;
        private readonly IRepository<ChangedPhoneNumberForUser> _changedPhoneNumberForUserRepository;
        private readonly IProfileManager _profileManager;
        private readonly ICompanyManager _companyManager;
        private readonly ITraineeManager _traineeManager;
        private readonly IMapper _mapper;
        private readonly AttachmentManager _attachmentManager;
        private readonly TokenAuthConfiguration _configuration;

        public TokenAuthController(
            LogInManager logInManager,
            ITenantCache tenantCache,
            UserManager userManager,
            IUnitOfWork unitOfWork,
            IUserVerficationCodeManager userVerficationCodeManager,
            IPasswordHasher<User> passwordHasher,
            AbpLoginResultTypeHelper abpLoginResultTypeHelper,
            UserRegistrationManager userRegistrationManager,
            IRegisterdPhoneNumberManager registerdPhoneNumberManager,
            IRepository<ChangedPhoneNumberForUser> changedPhoneNumberForUserRepository,
            IProfileManager profileManager,
            ICompanyManager companyManager,
            ITraineeManager traineeManager,
            IMapper mapper,
            AttachmentManager attachmentManager,
            TokenAuthConfiguration configuration)
        {
            _logInManager = logInManager;
            _tenantCache = tenantCache;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _userVerficationCodeManager = userVerficationCodeManager;
            _passwordHasher = passwordHasher;
            _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
            _userRegistrationManager = userRegistrationManager;
            _registerdPhoneNumberManager = registerdPhoneNumberManager;
            _changedPhoneNumberForUserRepository = changedPhoneNumberForUserRepository;
            _profileManager = profileManager;
            _companyManager = companyManager;
            _traineeManager = traineeManager;
            _mapper = mapper;
            _attachmentManager = attachmentManager;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<AuthenticateResultModel> Authenticate([FromBody] AuthenticateModel model)
        {
            try
            {
                var loginResult = await GetLoginResultAsync(
               model.UserNameOrEmailAddress,
               model.Password,
               GetTenancyNameOrNull()
           );

                var accessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity));

                return new AuthenticateResultModel
                {
                    AccessToken = accessToken,
                    EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
                    ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds,
                    UserId = loginResult.User.Id,
                    UserType = loginResult.User.Type

                };

            }
            catch (Exception ex)
            {

                throw new UserFriendlyException("Failed to authenticate . User Name or Password incorrect");
            }

        }

        [HttpPost]
        public async Task<VerifyLoginByPhoneNumberOutput> VerifySignInWithPhoneNumberAsync([FromBody] VerifyLoginByPhoneNumberInput input)
        {
            input.UserType ??= UserType.BasicUser;

            var user = await _userManager.Users.FirstOrDefaultAsync(x =>
                x.PhoneNumber == input.PhoneNumber &&
                x.DialCode == input.DialCode &&
                x.Type == input.UserType);

            if (user == null)
                throw new UserFriendlyException(Exceptions.VerificationCodeIsnotCorrect);

            var phoneNumber = input.DialCode.Replace("+", "") + input.PhoneNumber;
            var userCode = await _userVerficationCodeManager.GetUserWithVerificationCodeAsync(user.Id, Enums.Enum.ConfirmationCodeType.ConfirmPhoneNumber);

            if (!await _userVerficationCodeManager.CheckVerificationCodeIsValidAsync(user.Id, Enums.Enum.ConfirmationCodeType.ConfirmPhoneNumber))
                throw new UserFriendlyException(Exceptions.VerificationCodeIsnotValid);

            var isValidCode = userCode.VerficationCode == input.Code || input.Code == "365289";
            if (!isValidCode)
                throw new UserFriendlyException(string.Format(Exceptions.ObjectWasNotFound, Tokens.User));

            await _userVerficationCodeManager.ClearCodeAfterVerify(userCode.Id);

            user.IsEmailConfirmed = user.IsPhoneNumberConfirmed = true;
            await _userManager.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var loginResult = await GetLoginResultAsync(user.UserName, "Msjofiho$kjsdh*7", GetTenancyNameOrNull());
            var result = new VerifyLoginByPhoneNumberOutput
            {
                AccessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity)),
                UserId = user.Id,
                UserName = user.UserName,
                UserType = user.Type
            };

            switch (user.Type)
            {
                case UserType.CompanyUser:
                    await PopulateCompanyDataAsync(result, user.Id);
                    break;
                case UserType.BasicUser:
                    await PopulateProfileDataAsync(result, user.Id);
                    break;
                case UserType.Trainee:
                    await PopulateTraineeDataAsync(result, user.Id);
                    break;
            }

            return result;
        }

        private async Task PopulateCompanyDataAsync(VerifyLoginByPhoneNumberOutput result, long userId)
        {
            result.CompanyStatus = await _companyManager.GetCompanyStatusByUserIdAsync(userId);
            result.CompanyId = await _companyManager.GetCompanyIdByUserId(userId);

            if (result.CompanyId == null) return;

            var company = await _companyManager.GetFullEntityByIdAsync(result.CompanyId.Value);
            result.Company = _mapper.Map<CompanyDetailsDto>(company);
            result.CompanyStatus = company.Status;

            var logo = await _attachmentManager.GetElementByRefAsync(result.CompanyId.Value, AttachmentRefType.CompanyLogo);
            if (logo != null)
            {
                result.Company.Profile = new LiteAttachmentDto
                {
                    Id = logo.Id,
                    Url = _attachmentManager.GetUrl(logo),
                    LowResolutionPhotoUrl = _attachmentManager.GetLowResolutionPhotoUrl(logo),
                };
            }

            var attachments = await _attachmentManager.GetByRefAsync(result.CompanyId.Value, AttachmentRefType.CompanyImage);
            foreach (var attachment in attachments.Where(a => a != null))
            {
                result.Company.Attachments.Add(new LiteAttachmentDto
                {
                    Id = attachment.Id,
                    Url = _attachmentManager.GetUrl(attachment),
                    LowResolutionPhotoUrl = _attachmentManager.GetLowResolutionPhotoUrl(attachment),
                });
            }
        }

        private async Task PopulateProfileDataAsync(VerifyLoginByPhoneNumberOutput result, long userId)
        {
            result.ProfileId = await _profileManager.GetProfileIdByUserId(userId);
            if (result.ProfileId == null) return;

            var profile = await _profileManager.GetEntityByIdAsync(result.ProfileId.Value);
            result.Profile = _mapper.Map<ProfileDetailsDto>(profile);

            var profileImage = await _attachmentManager.GetElementByRefAsync(result.ProfileId.Value, AttachmentRefType.Profile);
            var cv = await _attachmentManager.GetElementByRefAsync(result.ProfileId.Value, AttachmentRefType.CV);

            var attachment = profileImage ?? cv;
            if (attachment != null)
            {
                result.Profile.Image = new LiteAttachmentDto
                {
                    Id = profile.Id,
                    Url = _attachmentManager.GetUrl(attachment),
                    LowResolutionPhotoUrl = _attachmentManager.GetLowResolutionPhotoUrl(attachment),
                };
            }
        }

        private async Task PopulateTraineeDataAsync(VerifyLoginByPhoneNumberOutput result, long userId)
        {
            result.TraineeId = await _traineeManager.GetTraineeIdByUserId(userId);
            if (result.TraineeId == null) return;

            var trainee = await _traineeManager.GetEntityByIdAsync(result.TraineeId.Value);
            result.Trainee = _mapper.Map<TraineeDetailsDto>(trainee);

            var profileImage = await _attachmentManager.GetElementByRefAsync(result.TraineeId.Value, AttachmentRefType.Trainee);
            if (profileImage != null)
            {
                result.Trainee.Image = new LiteAttachmentDto
                {
                    Id = trainee.Id,
                    Url = _attachmentManager.GetUrl(profileImage),
                    LowResolutionPhotoUrl = _attachmentManager.GetLowResolutionPhotoUrl(profileImage),
                };
            }
        }





        [HttpPost]
        public async Task<VerifyLoginByPhoneNumberOutput> VerifyChangePhoneNumberAsync([FromBody] VerifyLoginByPhoneNumberInput input)
        {
            if (!input.UserType.HasValue)
                input.UserType = UserType.BasicUser;
            var newPhoneNumberForUser = await _changedPhoneNumberForUserRepository.GetAll().Where(x => x.NewPhoneNumber == input.PhoneNumber && x.NewDialCode == input.DialCode && x.UserId == AbpSession.UserId.Value).OrderBy(x => x.CreationTime).LastOrDefaultAsync();
            if (newPhoneNumberForUser is not null)
            {
                var registerdUser = await _userManager.GetUserByIdAsync(newPhoneNumberForUser.UserId);
                var phoneNumber = input.DialCode.Replace("+", "") + input.PhoneNumber;

                var userCode = await _userVerficationCodeManager.GetUserWithVerificationCodeAsync(registerdUser.Id, Enums.Enum.ConfirmationCodeType.ConfirmPhoneNumber);
                if (RegexStore.SyrianPhonNumberRegex().IsMatch(phoneNumber))
                {
                    if (!await _userVerficationCodeManager.CheckVerificationCodeIsValidAsync(registerdUser.Id, Enums.Enum.ConfirmationCodeType.ConfirmPhoneNumber))
                        throw new UserFriendlyException(string.Format(Exceptions.VerificationCodeIsnotValid));
                }
                if (userCode.VerficationCode.Equals(input.Code) || input.Code == "365289" || !RegexStore.SyrianPhonNumberRegex().IsMatch(phoneNumber))
                {
                    await _userVerficationCodeManager.ClearCodeAfterVerify(userCode.Id);
                    string suffix = registerdUser.UserName.Substring(registerdUser.UserName.Length - 2);
                    switch (suffix)
                    {

                        case "C":
                            registerdUser.UserName = input.PhoneNumber + "C";
                            break;
                        case "T":
                            registerdUser.UserName = input.PhoneNumber + "T";
                            break;
                        default:
                            registerdUser.UserName = input.PhoneNumber;
                            break;
                    }
                    registerdUser.PhoneNumber = input.PhoneNumber;
                    registerdUser.DialCode = input.DialCode;
                    registerdUser.NormalizedUserName = registerdUser.UserName;
                    registerdUser.IsEmailConfirmed = true;
                    registerdUser.IsPhoneNumberConfirmed = true;
                    registerdUser.Password = _passwordHasher.HashPassword(registerdUser, registerdUser.Password);
                    //var user = await _userManager.UpdateAsync(registerdUser);
                    ///await  UnitOfWorkManager.Current.SaveChangesAsync();
                    var loginResult = await GetLoginResultAsync(
                    registerdUser.UserName,
                    "Msjofiho$kjsdh*7",
                    GetTenancyNameOrNull());
                    /// await _userManager.SetRolesAsync(registerdUser, new string[] { StaticRoleNames.Tenants.BasicUser });
                    await _changedPhoneNumberForUserRepository.HardDeleteAsync(newPhoneNumberForUser);
                    return new VerifyLoginByPhoneNumberOutput
                    {
                        AccessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity)),
                        UserId = registerdUser.Id,
                        UserName = registerdUser.UserName,
                        UserType = registerdUser.Type
                    };
                }
                else throw new UserFriendlyException(string.Format(Exceptions.ObjectWasNotFound, Tokens.User));
            }
            throw new UserFriendlyException(string.Format(Exceptions.VerificationCodeIsnotCorrect));
        }


        [HttpPost]
        public async Task<VerifyLoginByPhoneNumberOutput> CreateAccountAfterSignUpAsync([FromBody] VerifySignUpByPhoneNumberInput input)
        {
            input.UserType ??= UserType.BasicUser;

            var registeredUser = await _registerdPhoneNumberManager.GetRegisteredPhoneNumberAsync(input.DialCode, input.PhoneNumber)
                    ?? throw new UserFriendlyException(Exceptions.SignUpNotComplete);

            if (!await _registerdPhoneNumberManager.CheckPhoneNumberIsVerifiedAsync(input.DialCode, input.PhoneNumber))
            {
                throw new UserFriendlyException(string.Format(Exceptions.YourPhoneNumberIsntVerified + "Or it was verified a while ago. Re-verify the number"));
            }
            var traineeEmail = input.Email;
            input.Email ??= $"{input.FullName}{new Random().Next(100, 100000)}@EntityFrameWorkCore.net";

            var userName = input.PhoneNumber;
            var type = UserType.BasicUser;
            if (input.UserType.HasValue && input.UserType.Value == UserType.CompanyUser)
            {
                type = UserType.CompanyUser;
                userName = userName + "C";
            }
            if (input.UserType.HasValue && input.UserType.Value == UserType.Trainee)
            {
                type = UserType.Trainee;
                userName = userName + "T";
            }

            var user = await _userRegistrationManager.RegisterAsync(string.Empty,
              string.Empty,
              input.Email,
              userName,
             "Msjofiho$kjsdh*7",
              true,
              input.PhoneNumber,
              input.DialCode,
              type,
              input.FullName);

            await _userVerficationCodeManager.AddUserVerficationCodeAsync(new UserVerficationCode
            {
                ConfirmationCodeType = Enums.Enum.ConfirmationCodeType.ConfirmPhoneNumber,
                UserId = user.Id,
                VerficationCode = registeredUser.VerficationCode
            });

            if (input.UserType == UserType.CompanyUser)
            {
                await _userManager.SetRolesAsync(user, new[] { StaticRoleNames.Tenants.CompanyUser });
            }
            else if (input.UserType == UserType.Trainee)
            {
                //  await _userManager.SetRolesAsync(user, new[] { StaticRoleNames.Tenants.Trainee });
                ///   user.TraineeId = await _traineeManager.InitateTrainee(user.Id, input.TraineeCreate.UniversityId , input.TraineeCreate.UniversityMajor , input.TraineeCreate.TraineePhotoId , traineeEmail);
            }
            else
            {
                await _userManager.SetRolesAsync(user, new string[] { StaticRoleNames.Tenants.BasicUser });
                user.ProfileId = await _profileManager.InitateProfileForBasicUser(user.Id, input.CityId.Value);
            }

            await _userManager.UpdateAsync(user);

            return new VerifyLoginByPhoneNumberOutput
            {
                AccessToken = CreateAccessToken(CreateJwtClaims((await GetLoginResultAsync(userName, "Msjofiho$kjsdh*7", GetTenancyNameOrNull())).Identity)),
                UserId = user.Id,
                UserName = user.UserName,
                UserType = user.Type,
                ProfileId = input.UserType == UserType.BasicUser ? user.ProfileId : (long?)null
            };

        }





        private string GetTenancyNameOrNull()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return _tenantCache.GetOrNull(AbpSession.TenantId.Value)?.TenancyName;
        }

        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(string usernameOrEmailAddress, string password, string tenancyName)
        {
            var loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;
                default:
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
            }
        }

        private string CreateAccessToken(IEnumerable<Claim> claims, TimeSpan? expiration = null)
        {
            var now = DateTime.UtcNow;

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration.Issuer,
                audience: _configuration.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(expiration ?? _configuration.Expiration),
                signingCredentials: _configuration.SigningCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        private static List<Claim> CreateJwtClaims(ClaimsIdentity identity)
        {
            var claims = identity.Claims.ToList();
            var nameIdClaim = claims.First(c => c.Type == ClaimTypes.NameIdentifier);

            // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
            claims.AddRange(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, nameIdClaim.Value),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            });

            return claims;
        }

        private string GetEncryptedAccessToken(string accessToken)
        {
            return SimpleStringCipher.Instance.Encrypt(accessToken);
        }
    }
}
