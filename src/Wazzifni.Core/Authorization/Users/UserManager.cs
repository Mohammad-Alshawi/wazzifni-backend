using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Organizations;
using Abp.Runtime.Caching;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wazzifni.Authorization.Roles;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Authorization.Users
{
    public class UserManager : AbpUserManager<Role, User>
    {
        private readonly IRepository<User, long> _userRepository;

        public UserManager(
          RoleManager roleManager,
          UserStore store,
          IOptions<IdentityOptions> optionsAccessor,
          IPasswordHasher<User> passwordHasher,
          IEnumerable<IUserValidator<User>> userValidators,
          IEnumerable<IPasswordValidator<User>> passwordValidators,
          ILookupNormalizer keyNormalizer,
          IdentityErrorDescriber errors,
          IServiceProvider services,
          IRepository<User, long> UserRepository,
          ILogger<UserManager<User>> logger,
          IPermissionManager permissionManager,
          IUnitOfWorkManager unitOfWorkManager,
          ICacheManager cacheManager,
          IRepository<OrganizationUnit, long> organizationUnitRepository,
          IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
          IOrganizationUnitSettings organizationUnitSettings,
          ISettingManager settingManager,
          IRepository<UserLogin, long> userLoginRepository)
          : base(
              roleManager,
              store,
              optionsAccessor,
              passwordHasher,
              userValidators,
              passwordValidators,
              keyNormalizer,
              errors,
              services,
              logger,
              permissionManager,
              unitOfWorkManager,
              cacheManager,
              organizationUnitRepository,
              userOrganizationUnitRepository,
              organizationUnitSettings,
              settingManager,
              userLoginRepository)
        {
            _userRepository = UserRepository;
        }

        public async Task<bool> CheckUserTypeFromSession(UserType userType)
        {
            if (AbpSession.UserId.HasValue)
            {
                var dbUser = await _userRepository.FirstOrDefaultAsync(AbpSession.UserId.Value);

                return dbUser.Type == userType;
            }

            return false;
        }

        public async Task<bool> CheckUserTypeByUserId(UserType userType, long UserId)
        {

            var dbUser = await _userRepository.FirstOrDefaultAsync(UserId);

            return dbUser.Type == userType;

        }

        public async Task<bool> IsCompany() => await CheckUserTypeFromSession(UserType.CompanyUser);

        public async Task<bool> IsAdminSession() => await CheckUserTypeFromSession(UserType.Admin);
        public async Task<bool> IsBasicUser() => await CheckUserTypeFromSession(UserType.BasicUser);

        public async Task<bool> IsTrainee() => await CheckUserTypeFromSession(UserType.Trainee);

        public async Task<bool> IsCompany(long UserId) => await CheckUserTypeByUserId(UserType.CompanyUser, UserId);
        public async Task<bool> IsBasicUser(long UserId) => await CheckUserTypeByUserId(UserType.BasicUser, UserId);
        public async Task<bool> IsTrainee(long UserId) => await CheckUserTypeByUserId(UserType.Trainee, UserId);

    }
}
