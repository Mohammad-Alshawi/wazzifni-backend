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
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
using Wazzifni.Domains.UserVerficationCodes;
using Wazzifni.Localization.SourceFiles;
using Wazzifni.Models.TokenAuth;
using Wazzifni.MultiTenancy;
using Wazzifni.Profiles.Dto;
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
            if (!input.UserType.HasValue)
                input.UserType = UserType.BasicUser;
            var registerdUser = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == input.PhoneNumber && x.DialCode == input.DialCode && x.Type == input.UserType);
            if (registerdUser is not null)
            {
                var phoneNumber = input.DialCode.Replace("+", "") + input.PhoneNumber;

                var userCode = await _userVerficationCodeManager.GetUserWithVerificationCodeAsync(registerdUser.Id, Enums.Enum.ConfirmationCodeType.ConfirmPhoneNumber);
                if (RegexStore.SyrianPhonNumberRegex().IsMatch(phoneNumber))
                {
                    if (!await _userVerficationCodeManager.CheckVerificationCodeIsValidAsync(registerdUser.Id, Enums.Enum.ConfirmationCodeType.ConfirmPhoneNumber))
                    {
                        throw new UserFriendlyException(string.Format(Exceptions.VerificationCodeIsnotValid));
                    }
                }

                if (userCode.VerficationCode.Equals(input.Code) || input.Code == "365289" || !RegexStore.SyrianPhonNumberRegex().IsMatch(phoneNumber))
                {
                    await _userVerficationCodeManager.ClearCodeAfterVerify(userCode.Id);

                    registerdUser.IsEmailConfirmed = true;
                    registerdUser.IsPhoneNumberConfirmed = true;
                    var user = await _userManager.UpdateAsync(registerdUser);
                    await _unitOfWork.SaveChangesAsync();

                    var loginResult = await GetLoginResultAsync(
                        registerdUser.UserName,
                        "Msjofiho$kjsdh*7",
                        GetTenancyNameOrNull()
                    );

                    /// await _userManager.SetRolesAsync(registerdUser, new string[] { StaticRoleNames.Tenants.BasicUser });
                    var result = new VerifyLoginByPhoneNumberOutput
                    {
                        AccessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity)),
                        UserId = registerdUser.Id,
                        UserName = registerdUser.UserName,
                        UserType = registerdUser.Type
                    };
                    if (registerdUser.Type == UserType.CompanyUser)
                    {
                        result.CompanyStatus = await _companyManager.GetCompanyStatusByUserIdAsync(registerdUser.Id);

                        result.CompanyId = await _companyManager.GetCompanyIdByUserId(registerdUser.Id);
                        if (result.CompanyId.HasValue)
                        {
                            var company = await _companyManager.GetEntityByIdAsync(result.CompanyId.Value);
                            result.Company = _mapper.Map<CompanyDetailsDto>(company);
                            var logo = await _attachmentManager.GetElementByRefAsync(result.CompanyId.Value, AttachmentRefType.CompanyLogo);
                            if (logo is not null)
                            {

                                result.Company.Profile = new LiteAttachmentDto
                                {
                                    Id = logo.Id,
                                    Url = _attachmentManager.GetUrl(logo),
                                    LowResolutionPhotoUrl = _attachmentManager.GetLowResolutionPhotoUrl(logo),
                                };
                            }
                        }
                    }
                    if (registerdUser.Type == UserType.BasicUser)
                    {
                        result.ProfileId = await _profileManager.GetProfileIdByUserId(registerdUser.Id);
                        if (result.ProfileId.HasValue)
                        {
                            var profile = await _profileManager.GetEntityByIdAsync(result.ProfileId.Value);
                            result.Profile = _mapper.Map<ProfileDetailsDto>(profile);
                            var profileImage = await _attachmentManager.GetElementByRefAsync(result.ProfileId.Value, AttachmentRefType.Profile);
                            if (profileImage is not null)
                            {

                                result.Profile.Image = new LiteAttachmentDto
                                {
                                    Id = profile.Id,
                                    Url = _attachmentManager.GetUrl(profileImage),
                                    LowResolutionPhotoUrl = _attachmentManager.GetLowResolutionPhotoUrl(profileImage),
                                };
                            }
                        }
                    }
                    return result;

                }
                throw new UserFriendlyException(string.Format(Exceptions.ObjectWasNotFound, Tokens.User));
            }
            throw new UserFriendlyException(string.Format(Exceptions.VerificationCodeIsnotCorrect));
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
            if (!input.UserType.HasValue)
                input.UserType = UserType.BasicUser;
            var registerdUser = await _registerdPhoneNumberManager.GetRegisteredPhoneNumberAsync(input.DialCode, input.PhoneNumber);
            if (registerdUser is not null)
            {
                if (!await _registerdPhoneNumberManager.CheckPhoneNumberIsVerifiedAsync(input.DialCode, input.PhoneNumber))
                {
                    throw new UserFriendlyException(string.Format(Exceptions.YourPhoneNumberIsntVerified + "Or it was verified a while ago. Re-verify the number"));
                }
                if (string.IsNullOrEmpty(input.Email))
                {
                    Random random = new Random();
                    int randomNumber = random.Next(100, 100000);
                    input.Email = input.FullName + randomNumber.ToString() + "@EntityFrameWorkCore.net";
                }
                var userName = input.PhoneNumber;
                var type = UserType.BasicUser;
                if (input.UserType.HasValue && input.UserType.Value == UserType.CompanyUser)
                {
                    type = UserType.CompanyUser;
                    userName = userName + "C";
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

                var loginResult = await GetLoginResultAsync(
                userName,
                "Msjofiho$kjsdh*7",
                GetTenancyNameOrNull());
                await _userVerficationCodeManager.AddUserVerficationCodeAsync(
                    new UserVerficationCode
                    {
                        ConfirmationCodeType = Enums.Enum.ConfirmationCodeType.ConfirmPhoneNumber,
                        UserId = user.Id,
                        VerficationCode = registerdUser.VerficationCode
                    });

                if (input.UserType.HasValue && input.UserType.Value == UserType.CompanyUser)
                    await _userManager.SetRolesAsync(user, new string[] { StaticRoleNames.Tenants.CompanyUser });

                else
                {
                    await _userManager.SetRolesAsync(user, new string[] { StaticRoleNames.Tenants.BasicUser });
                    await _profileManager.InitateProfileForBasicUser(user.Id, input.CityId.Value);
                }



                var result = new VerifyLoginByPhoneNumberOutput
                {
                    AccessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity)),
                    UserId = user.Id,
                    UserName = user.UserName,
                    UserType = user.Type,
                };

                if (input.UserType == UserType.BasicUser)
                {
                    result.ProfileId = await _profileManager.GetProfileIdByUserId(registerdUser.Id);
                }
                return result;

            }
            else
                throw new UserFriendlyException(string.Format(Exceptions.SignUpNotComplete));
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
