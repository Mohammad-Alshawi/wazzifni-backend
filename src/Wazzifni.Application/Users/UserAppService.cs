using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Abp.Localization;
using Abp.Notifications;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wazzifni.Authorization;
using Wazzifni.Authorization.Roles;
using Wazzifni.Authorization.Users;
using Wazzifni.Domain.Attachments;
using Wazzifni.Localization.SourceFiles;
using Wazzifni.NotificationService;
using Wazzifni.Roles.Dto;
using Wazzifni.Users.Dto;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Users
{
    [AbpAuthorize(PermissionNames.Pages_Users)]
    public class UserAppService : AsyncCrudAppService<User, UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>, IUserAppService
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IRepository<Role> _roleRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAbpSession _abpSession;
        private readonly LogInManager _logInManager;
        private readonly IUserNotificationManager _userNotificationManager;
        private readonly IRepository<UserLoginAttempt, long> _userLoginAttemptRepository;
        private readonly IRepository<AuditLog, long> _auditLogRepository;
        private readonly IAttachmentManager _attachmentManager;
        private readonly FirebaseNotificationService _firebaseNotificationService;
        private readonly ISettingManager _settingManager;

        private static readonly Dictionary<UserType, string[]> UserTypeRoleMapping = new()
        {
            { UserType.BasicUser, new[] { StaticRoleNames.Tenants.BasicUser } },
            { UserType.CompanyUser, new[] { StaticRoleNames.Tenants.CompanyUser } },
            { UserType.Trainee, new[] { StaticRoleNames.Tenants.Trainee }}
        };
        public UserAppService(
            IRepository<User, long> repository,
            UserManager userManager,
            RoleManager roleManager,
            IRepository<Role> roleRepository,
            IPasswordHasher<User> passwordHasher,
            IAbpSession abpSession,
            LogInManager logInManager,
            IUserNotificationManager userNotificationManager,
            IRepository<UserLoginAttempt, long> userLoginAttemptRepository,
            IRepository<AuditLog, long> auditLogRepository,
            IAttachmentManager attachmentManager,
            FirebaseNotificationService firebaseNotificationService,
            ISettingManager settingManager
        )
            : base(repository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _abpSession = abpSession;
            _logInManager = logInManager;
            _userNotificationManager = userNotificationManager;
            _userLoginAttemptRepository = userLoginAttemptRepository;
            _auditLogRepository = auditLogRepository;
            _attachmentManager = attachmentManager;
            _firebaseNotificationService = firebaseNotificationService;
            _settingManager = settingManager;
        }
        [AbpAuthorize(PermissionNames.Users_List)]
        public override async Task<PagedResultDto<UserDto>> GetAllAsync(PagedUserResultRequestDto input)
        {
            var userLogin = await _userManager.GetUserByIdAsync(AbpSession.UserId.Value);
            var userWhoHasRoleIds = new List<long>();
            if (input.RoleNames is not null)
            {
                foreach (var role in input.RoleNames)
                {
                    var usersInRole = await _userManager.GetUsersInRoleAsync(role);
                    var usersInRoleIds = usersInRole.Select(x => x.Id).ToList();
                    userWhoHasRoleIds.AddRange(usersInRoleIds);
                }

            }
            input.UserIds = userWhoHasRoleIds;
            var result = await base.GetAllAsync(input);
            var lastTimesActivation = await _auditLogRepository.GetAll().AsNoTracking().Where(x => result.Items.Select(x => x.Id).Contains(x.UserId.Value)).ToListAsync();
            var lastLogins = await _userLoginAttemptRepository.GetAll().AsNoTracking().Where(attempt => result.Items.Select(x => x.Id).Contains(attempt.UserId.Value)).ToListAsync();
            foreach (var user in result?.Items)
            {
                user.LastLoginTime = lastLogins.Where(x => x.UserId == user.Id).Select(x => x.CreationTime).LastOrDefault();
                user.LastActivationTime = lastTimesActivation.Where(x => x.UserId == user.Id).Select(x => x.ExecutionTime).LastOrDefault();
                Attachment attachment = await _attachmentManager.GetElementByRefAsync(user.Id, Enums.Enum.AttachmentRefType.Profile);
                if (attachment is not null)
                {
                    user.ProfilePhoto = new LiteAttachmentDto
                    {

                        Id = attachment.Id,
                        Url = _attachmentManager.GetUrl(attachment),
                        LowResolutionPhotoUrl = _attachmentManager.GetLowResolutionPhotoUrl(attachment),
                    };
                }

            }
            return result;

        }

        [AbpAuthorize(PermissionNames.Users_Create)]

        public override async Task<UserDto> CreateAsync(CreateUserDto input)
        {

            CheckCreatePermission();

            var user = ObjectMapper.Map<User>(input);

            user.TenantId = AbpSession.TenantId;
            user.IsEmailConfirmed = true;
            user.Type = input.UserType;
            var roleNames = _roleRepository.GetAll().Select(x => x.Name).ToList();
            await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

            CheckErrors(await _userManager.CreateAsync(user, input.Password));


          

            if (input.RoleNames != null)
            {
                foreach (var rolename in input.RoleNames)
                {
                    if (!roleNames.Contains(rolename))
                    {
                        throw new UserFriendlyException(string.Format(Exceptions.ObjectWasNotFound, Tokens.Role + " " + rolename));
                    }
                    if (input.UserType != UserType.Admin)
                    {
                        var allowedRoles = UserTypeRoleMapping[user.Type];

                        if(!allowedRoles.Contains(rolename))

                        throw new UserFriendlyException($"UserType '{user.Type}' is not allowed to have role '{rolename}'.");
                    }
                }
                CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
            }

            CurrentUnitOfWork.SaveChanges();

            return MapToEntityDto(user);
        }

        [AbpAuthorize(PermissionNames.Users_Update)]
        public override async Task<UserDto> UpdateAsync(UserDto input)
        {
            CheckUpdatePermission();
            var userLogin = await _userManager.GetUserByIdAsync(AbpSession.UserId.Value);
            if (userLogin.Type != UserType.Admin)
                throw new UserFriendlyException(403, Exceptions.YouCannotDoThisAction, "Only Admins ");
            var user = await _userManager.GetUserByIdAsync(input.Id);

            MapToEntity(input, user);

            CheckErrors(await _userManager.UpdateAsync(user));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
            }

            return await GetAsync(input);
        }

        [AbpAuthorize(PermissionNames.Users_Delete)]
        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);
            await _userManager.DeleteAsync(user);
        }

        [AbpAuthorize(PermissionNames.Pages_Users_Activation)]
        public async Task Activate(EntityDto<long> user)
        {
            await Repository.UpdateAsync(user.Id, async (entity) =>
            {
                entity.IsActive = true;
            });
        }

        [AbpAuthorize(PermissionNames.Pages_Users_Activation)]
        public async Task DeActivate(EntityDto<long> user)
        {
            await Repository.UpdateAsync(user.Id, async (entity) =>
            {
                entity.IsActive = false;
            });
        }

        [AbpAuthorize(PermissionNames.Roles_List)]
        public async Task<ListResultDto<RoleDto>> GetRoles()
        {
            var roles = await _roleRepository.GetAllListAsync();
            return new ListResultDto<RoleDto>(ObjectMapper.Map<List<RoleDto>>(roles));
        }

        public async Task ChangeLanguage(ChangeUserLanguageDto input)
        {
            await SettingManager.ChangeSettingForUserAsync(
                AbpSession.ToUserIdentifier(),
                LocalizationSettingNames.DefaultLanguage,
                input.LanguageName
            );
        }

        protected override User MapToEntity(CreateUserDto createInput)
        {
            var user = ObjectMapper.Map<User>(createInput);
            user.SetNormalizedNames();
            return user;
        }

        protected override void MapToEntity(UserDto input, User user)
        {
            ObjectMapper.Map(input, user);
            user.SetNormalizedNames();
        }

        protected override UserDto MapToEntityDto(User user)
        {
            var roleIds = user.Roles.Select(x => x.RoleId).ToArray();

            var roles = _roleManager.Roles.Where(r => roleIds.Contains(r.Id)).Select(r => r.NormalizedName);

            var userDto = base.MapToEntityDto(user);
            userDto.RoleNames = roles.ToArray();

            return userDto;
        }

        protected override IQueryable<User> CreateFilteredQuery(PagedUserResultRequestDto input)
        {
            var data = Repository.GetAllIncluding(x => x.Roles);


            data = data.WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.UserName.Contains(input.Keyword) || x.Name.Contains(input.Keyword) || x.EmailAddress.Contains(input.Keyword) || x.RegistrationFullName.Contains(input.Keyword) || x.Surname.Contains(input.Keyword) || x.Id.ToString().Contains(input.Keyword))
                      .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive)
                      .WhereIf(input.UserType.HasValue, x => x.Type == input.UserType.Value);

            if (input.CreateDateFrom.HasValue && input.CreateDateTo.HasValue)
                data = data.Where(x => x.CreationTime.Date >= input.CreateDateFrom.Value.Date && x.CreationTime.Date <= input.CreateDateTo.Value.Date).AsNoTracking();
            return data;


        }

        protected override async Task<User> GetEntityByIdAsync(long id)
        {
            var user = await Repository.GetAllIncluding(x => x.Roles).FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(User), id);
            }

            return user;
        }

        protected override IQueryable<User> ApplySorting(IQueryable<User> query, PagedUserResultRequestDto input)
        {
            return query.OrderBy(r => r.UserName);
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        public async Task<bool> ChangePassword(ChangePasswordDto input)
        {
            await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

            var user = await _userManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }

            if (await _userManager.CheckPasswordAsync(user, input.CurrentPassword))
            {
                CheckErrors(await _userManager.ChangePasswordAsync(user, input.NewPassword));
            }
            else
            {
                CheckErrors(IdentityResult.Failed(new IdentityError
                {
                    Description = "Incorrect password."
                }));
            }

            return true;
        }

        public async Task<bool> ResetPassword(ResetPasswordDto input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Please log in before attempting to reset password.");
            }

            var currentUser = await _userManager.GetUserByIdAsync(_abpSession.GetUserId());
            var loginAsync = await _logInManager.LoginAsync(currentUser.UserName, input.AdminPassword, shouldLockout: false);
            if (loginAsync.Result != AbpLoginResultType.Success)
            {
                throw new UserFriendlyException("Your 'Admin Password' did not match the one on record.  Please try again.");
            }

            if (currentUser.IsDeleted || !currentUser.IsActive)
            {
                return false;
            }

            var roles = await _userManager.GetRolesAsync(currentUser);
            if (!roles.Contains(StaticRoleNames.Tenants.Admin))
            {
                throw new UserFriendlyException("Only administrators may reset passwords.");
            }

            var user = await _userManager.GetUserByIdAsync(input.UserId);
            if (user != null)
            {
                user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
                await CurrentUnitOfWork.SaveChangesAsync();
            }

            return true;
        }

        /// <summary>
        /// Get FCM Token for current user, if exists
        /// </summary>
        [AbpAuthorize]
        public async Task<string> GetCurrentFcmTokenAsync()
        {
            var user = await GetEntityByIdAsync(AbpSession.UserId.Value);

            return user.FcmToken;
        }
        /// <summary>
        /// Set or clear FCM Token for current user
        /// </summary>
        [AbpAuthorize]
        public async Task SetCurrentFcmTokenAsync(string input)
        {
            var user = await GetEntityByIdAsync(AbpSession.UserId.Value);

            user.FcmToken = input;
            if (!input.IsNullOrWhiteSpace())
                await _firebaseNotificationService.SubscribeToTopic(new List<string> { user.FcmToken }, TopicType.All);

            if (await _userManager.IsBasicUser(user.Id))
                await _firebaseNotificationService.SubscribeToTopic(new List<string> { user.FcmToken }, TopicType.BasicUser);

            if (await _userManager.IsCompany(user.Id))
                await _firebaseNotificationService.SubscribeToTopic(new List<string> { user.FcmToken }, TopicType.CompanyUser);

            if (await _userManager.IsTrainee(user.Id))
                await _firebaseNotificationService.SubscribeToTopic(new List<string> { user.FcmToken }, TopicType.Trainee);

            if (user.Type == UserType.Admin)
                await _firebaseNotificationService.SubscribeToTopic(new List<string> { user.FcmToken }, TopicType.Admin);

            await Repository.UpdateAsync(user);
        }


        [AbpAuthorize(PermissionNames.Users_List)]
        public override async Task<UserDto> GetAsync(EntityDto<long> input)
        {
            var userLogin = await _userManager.GetUserByIdAsync(AbpSession.UserId.Value);
            var user = await base.GetAsync(input);
            user.LastLoginTime = await _userLoginAttemptRepository.GetAll()
                .AsNoTracking()
        .Where(attempt => attempt.UserId == user.Id)
            .OrderByDescending(attempt => attempt.CreationTime).Select(attempt => attempt.CreationTime)
            .FirstOrDefaultAsync();
            user.LastActivationTime = await _auditLogRepository.GetAll()
                .AsNoTracking()
         .Where(attempt => attempt.UserId == user.Id)
         .OrderByDescending(attempt => attempt.ExecutionTime).Select(attempt => attempt.ExecutionTime)
         .FirstOrDefaultAsync();

            for (int i = 0; i < user.RoleNames.Length; i++)
            {
                var role = await _roleManager.GetRoleByNameAsync(user.RoleNames[i]);
                user.RoleNames[i] = role.Name;
            }
            if (userLogin.Type == UserType.Admin && userLogin.UserName == "keyFinderAdmin-2")
            {
                user.UserName = null;
                user.PhoneNumber = null;
            }

            return user;
        }


        [AbpAuthorize(PermissionNames.Users_List)]
        public async Task<UserCountDto> GetUsersCount()
        {
            var users = await _userManager.Users.Include(x => x.Roles).ToListAsync();

            var userWhoHasRoleIds = new List<long>();

            var usersInAdmin = await _userManager.GetUsersInRoleAsync("Admin");
            var usersInAdminIds = usersInAdmin.Select(x => x.Id).ToList();
            userWhoHasRoleIds.AddRange(usersInAdminIds);

            var userCountDto = new UserCountDto
            {
                AllUsersCount = users.Count(),
                Admins = users.Where(u => (u.Type == UserType.Admin || userWhoHasRoleIds.Contains(u.Id)) && u.IsDeleted == false).Count(),
                Users = users.Where(u => u.Type != UserType.Admin && !userWhoHasRoleIds.Contains(u.Id) && u.IsDeleted == false).Count(),
                ActiveUsers = users.Where(u => u.IsActive == true && u.Type != UserType.Admin).Count(),
                DeActiveUsers = users.Where(u => u.IsActive == false && u.IsDeleted == false && u.Type != UserType.Admin).Count(),
                UsersJoinedThisMonth = users
                .Where(u => u.CreationTime.Year == DateTime.Now.Year
                && u.CreationTime.Month == DateTime.Now.Month).Count(),
            };
            return userCountDto;
        }


        [AbpAuthorize(PermissionNames.Users_List)]
        public async Task<List<InfoForUserChart>> GetInfoForUserChart(int? year)
        {
            if (!year.HasValue) year = DateTime.Now.Year;
            var users = await _userManager.Users.ToListAsync();
            var UserCountsByMonth = users
           .Where(u => u.CreationTime.Year == year.Value)
           .GroupBy(u => new { Month = u.CreationTime.Month })
           .Select(group => new InfoForUserChart
           {
               Month = group.Key.Month,
               UserCount = group.Count()
           })
           .OrderBy(result => result.Month)
           .ToList();
            return UserCountsByMonth;
        }


        public async Task<ListResultDto<string>> GetCurrentUserPermissionsAsync()
        {
            if (AbpSession.UserId == null)
            {
                throw new UserFriendlyException("Please log in before attempting to change password.");
            }
            // Get User own permissions
            var user = await GetEntityByIdAsync(AbpSession.UserId.Value);

            var permissions = (await _userManager.GetGrantedPermissionsAsync(user)).ToList();

            return new ListResultDto<string>(ObjectMapper.Map<List<string>>(permissions.OrderBy(p => p.Name).Distinct()));
        }




    }
}

