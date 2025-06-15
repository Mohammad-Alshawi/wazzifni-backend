using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.UI;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wazzifni.Authorization;
using Wazzifni.Authorization.Users;
using Wazzifni.Awards;
using Wazzifni.CrudAppServiceBase;
using Wazzifni.Domain.Attachments;
using Wazzifni.Domain.Educations;
using Wazzifni.Domain.IndividualUserProfiles;
using Wazzifni.Domain.Skills;
using Wazzifni.Domain.SpokenLanguages;
using Wazzifni.Domain.WorkApplications;
using Wazzifni.Domain.WorkExperiences;
using Wazzifni.Profiles.Dto;
using static Wazzifni.Enums.Enum;
using Profile = Wazzifni.Domain.IndividualUserProfiles.Profile;

namespace Wazzifni.Profiles
{
    public class ProfileAppService :
         WazzifniAsyncCrudAppService<Profile, ProfileDetailsDto, long, ProfileLiteDto, PagedProfileResultRequestDto, CreateProfileDto, UpdateProfileDto>,
         IProfileAppService
    {
        private readonly IRepository<Profile, long> _repository;
        private readonly IAttachmentManager _attachmentManager;
        private readonly UserManager _userManager;
        private readonly IProfileManager _profileManager;
        private readonly ISkillManager _skillManager;
        private readonly IRepository<Skill> _skillRepo;
        private readonly IRepository<Award, long> _awardRepo;
        private readonly IRepository<Education, long> _educationRepo;
        private readonly IRepository<WorkExperience, long> _workExperienceRepo;
        private readonly IRepository<SpokenLanguageValue, long> _spLanRepo;
        private readonly IRepository<WorkApplication, long> _workApplicationRepo;
        private readonly DeactivatedUsersSet _deactivatedUsersSet;
        private readonly IMapper _mapper;

        public ProfileAppService(IRepository<Profile, long> repository, IAttachmentManager attachmentManager, UserManager userManager,
            IProfileManager profileManager, ISkillManager skillManager,
            IRepository<Skill> skillRepo,
            IRepository<Award, long> awardRepo,
            IRepository<Education, long> educationRepo,
            IRepository<WorkExperience, long> workExperienceRepo,
            IRepository<SpokenLanguageValue, long> spLanRepo,
            IRepository<WorkApplication, long> workApplicationRepo,
            DeactivatedUsersSet deactivatedUsersSet,
            IMapper mapper) : base(repository)
        {
            _repository = repository;
            _attachmentManager = attachmentManager;
            _userManager = userManager;
            _profileManager = profileManager;
            _skillManager = skillManager;
            _skillRepo = skillRepo;
            _awardRepo = awardRepo;
            _educationRepo = educationRepo;
            _workExperienceRepo = workExperienceRepo;
            _spLanRepo = spLanRepo;
            _workApplicationRepo = workApplicationRepo;
            _deactivatedUsersSet = deactivatedUsersSet;
            _mapper = mapper;
        }


        public override async Task<ProfileDetailsDto> GetAsync(EntityDto<long> input)
        {
            var profile = await _profileManager.GetEntityByIdAsync(input.Id);

            var result = _mapper.Map<ProfileDetailsDto>(profile);
            var logo = await _attachmentManager.GetElementByRefAsync(result.Id, AttachmentRefType.Profile);
            if (logo is not null)
            {
                result.Image = new LiteAttachmentDto
                {
                    Id = logo.Id,
                    Url = _attachmentManager.GetUrl(logo),
                    LowResolutionPhotoUrl = _attachmentManager.GetLowResolutionPhotoUrl(logo),
                };
            }

            var cv = await _attachmentManager.GetElementByRefAsync(result.Id, AttachmentRefType.CV);
            if (cv is not null)
            {
                result.Cv = new LiteAttachmentDto
                {
                    Id = cv.Id,
                    Url = _attachmentManager.GetUrl(cv),
                    LowResolutionPhotoUrl = _attachmentManager.GetLowResolutionPhotoUrl(cv),
                };
            }
            return result;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [RemoteService(IsEnabled = false)]
        public override Task<ProfileDetailsDto> CreateAsync(CreateProfileDto input)
        {
            return base.CreateAsync(input);
        }

        [AbpAuthorize(PermissionNames.Users_Delete)]
        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var profile = await _profileManager.GetEntityByIdASTrackingAsync(input.Id);
            await BulkHardDeleteOldEntities(profile.Id);
            await _workApplicationRepo.DeleteAsync(x => x.ProfileId == input.Id);
            var user = await _userManager.GetUserByIdAsync(profile.UserId);
            await _userManager.DeleteAsync(user);
            await base.DeleteAsync(input);
        }


        [AbpAuthorize]
        public override async Task<ProfileDetailsDto> UpdateAsync(UpdateProfileDto input)
        {
            var profile = await _profileManager.GetEntityByUserIdAsync(AbpSession.UserId.Value);

            if (profile.Id != input.Id || !await _userManager.IsBasicUser())
            {
                throw new UserFriendlyException("Cant do this");
            }

            profile = _mapper.Map(input, profile);

            await BulkHardDeleteOldEntities(profile.Id);

            if (!input.SkillIds.IsNullOrEmpty())
            {
                var existingSkillIds = profile.Skills.Select(s => s.Id).ToList();
                var skillsToRemove = profile.Skills.Where(s => !input.SkillIds.Contains(s.Id)).ToList();
                var skillsToAddIds = input.SkillIds.Except(existingSkillIds).ToList();

                // Remove skills
                foreach (var skill in skillsToRemove)
                {
                    profile.Skills.Remove(skill);
                }

                // Add new skills
                foreach (var skillId in skillsToAddIds)
                {
                    var skill = await _skillManager.GetEntityByIdAsync(skillId);
                    profile.Skills.Add(skill);
                }
            }


            var logo = await _attachmentManager.GetElementByRefAsync(profile.Id, AttachmentRefType.Profile);

            if (input.ProfilePhotoId is null || input.ProfilePhotoId == 0)
            {
                if (logo is not null)
                    await _attachmentManager.DeleteRefIdAsync(logo);
            }
            else if (logo?.Id != input.ProfilePhotoId)
            {
                if (logo is not null)
                    await _attachmentManager.DeleteRefIdAsync(logo);

                await _attachmentManager.CheckAndUpdateRefIdAsync(input.ProfilePhotoId.Value, AttachmentRefType.Profile, profile.Id);
            }


            var cv = await _attachmentManager.GetElementByRefAsync(profile.Id, AttachmentRefType.CV);

            if (input.CvId == 0)
            {
                if (cv is not null)
                    await _attachmentManager.DeleteRefIdAsync(cv);
            }
            else if (cv?.Id != input.CvId)
            {
                if (cv is not null)
                    await _attachmentManager.DeleteRefIdAsync(cv);

                await _attachmentManager.CheckAndUpdateRefIdAsync(input.CvId, AttachmentRefType.CV, profile.Id);
            }



            await Repository.UpdateAsync(profile);

            profile.User.RegistrationFullName = input.RegistrationFullName;
            await _userManager.UpdateAsync(profile.User);

            UnitOfWorkManager.Current.SaveChanges();



            return _mapper.Map<ProfileDetailsDto>(profile);
        }

        [AbpAuthorize]
        public async Task UpdateBasicInfoAsync(UpdateBasicProfileDto input)
        {
            var profile = await _profileManager.GetEntityByUserIdAsync(AbpSession.UserId.Value);

            if (profile.Id != input.Id || !await _userManager.IsBasicUser())
            {
                throw new UserFriendlyException("Cant do this");
            }

            profile.CityId = input.CityId;
            profile.Gender = input.Gender;
            profile.User.RegistrationFullName = input.RegistrationFullName;

            await _userManager.UpdateAsync(profile.User);
            await Repository.UpdateAsync(profile);
        }

        [AbpAuthorize]
        public async Task UpdateSkillsAsync(UpdateSkillsDto input)
        {
            var profile = await _profileManager.GetEntityByUserIdAsync(AbpSession.UserId.Value);

            if (profile.Id != input.Id || !await _userManager.IsBasicUser())
            {
                throw new UserFriendlyException("Cant do this");
            }
            var existingSkillIds = profile.Skills.Select(s => s.Id).ToList();
            var skillsToRemove = profile.Skills.Where(s => !input.SkillIds.Contains(s.Id)).ToList();
            var skillsToAddIds = input.SkillIds.Except(existingSkillIds).ToList();

            foreach (var skill in skillsToRemove)
                profile.Skills.Remove(skill);

            foreach (var skillId in skillsToAddIds)
            {
                var skill = await _skillManager.GetEntityByIdAsync(skillId);
                profile.Skills.Add(skill);
            }

            await Repository.UpdateAsync(profile);
        }

        [AbpAuthorize]
        public async Task UpdateEducationsAsync(UpdateEducationsDto input)
        {
            var profile = await _profileManager.GetEntityByUserIdAsync(AbpSession.UserId.Value);

            if (profile.Id != input.Id || !await _userManager.IsBasicUser())
            {
                throw new UserFriendlyException("Cant do this");
            }
            await _educationRepo.HardDeleteAsync(x => x.ProfileId == profile.Id);
            profile.Educations = _mapper.Map<List<Education>>(input.Educations);
            await Repository.UpdateAsync(profile);
        }

        [AbpAuthorize]
        public async Task UpdateWorkExperiencesAsync(UpdateWorkExperiencesDto input)
        {
            var profile = await _profileManager.GetEntityByUserIdAsync(AbpSession.UserId.Value);

            if (profile.Id != input.Id || !await _userManager.IsBasicUser())
            {
                throw new UserFriendlyException("Cant do this");
            }
            await _workExperienceRepo.HardDeleteAsync(x => x.ProfileId == profile.Id);
            profile.WorkExperiences = _mapper.Map<List<WorkExperience>>(input.WorkExperiences);
            await Repository.UpdateAsync(profile);
        }

        [AbpAuthorize]
        public async Task UpdateAwardsAsync(UpdateAwardsDto input)
        {
            var profile = await _profileManager.GetEntityByUserIdAsync(AbpSession.UserId.Value);

            if (profile.Id != input.Id || !await _userManager.IsBasicUser())
            {
                throw new UserFriendlyException("Cant do this");
            }
            await _awardRepo.HardDeleteAsync(x => x.ProfileId == profile.Id);
            profile.Awards = _mapper.Map<List<Award>>(input.Awards);
            await Repository.UpdateAsync(profile);
        }


        [AbpAuthorize]
        public async Task UpdateSpokenLanguagesAsync(UpdateSpokenLanguagesDto input)
        {
            var profile = await _profileManager.GetEntityByUserIdAsync(AbpSession.UserId.Value);

            if (profile.Id != input.Id || !await _userManager.IsBasicUser())
            {
                throw new UserFriendlyException("Cant do this");
            }
            await _spLanRepo.HardDeleteAsync(x => x.ProfileId == profile.Id);
            profile.SpokenLanguages = _mapper.Map<List<SpokenLanguageValue>>(input.SpokenLanguages);
            await Repository.UpdateAsync(profile);

        }


        [AbpAuthorize]
        public async Task<ProfileDetailsDto> UpdateLogoAsync(long Id, [Required] long LogoAttchmentId)
        {
            if (!await _userManager.IsBasicUser())
            {
                throw new UserFriendlyException("Cant do this");
            }
            var profile = await _profileManager.GetEntityByUserIdAsync(AbpSession.UserId.Value);

            var logo = await _attachmentManager.GetElementByRefAsync(Id, AttachmentRefType.Profile);
            if (logo is not null && logo.Id != LogoAttchmentId && LogoAttchmentId != 0)
            {
                await _attachmentManager.DeleteRefIdAsync(logo);
                await _attachmentManager.CheckAndUpdateRefIdAsync(LogoAttchmentId, AttachmentRefType.Profile, profile.Id);

            }
            else if (logo is null && LogoAttchmentId != 0)
                await _attachmentManager.CheckAndUpdateRefIdAsync(LogoAttchmentId, AttachmentRefType.Profile, profile.Id);

            profile.LastModificationTime = DateTime.Now;
            await UnitOfWorkManager.Current.SaveChangesAsync();
            var result = MapToEntityDto(profile);
            return result;
        }


        [AbpAuthorize]
        public async Task<ProfileDetailsDto> UpdateCvAsync(long Id, [Required] long CvAttchmentId)
        {
            if (!await _userManager.IsBasicUser())
            {
                throw new UserFriendlyException("Cant do this");
            }
            var profile = await _profileManager.GetEntityByUserIdAsync(AbpSession.UserId.Value);

            var Cv = await _attachmentManager.GetElementByRefAsync(Id, AttachmentRefType.CV);
            if (Cv is not null && Cv.Id != CvAttchmentId && CvAttchmentId != 0)
            {
                await _attachmentManager.DeleteRefIdAsync(Cv);
                await _attachmentManager.CheckAndUpdateRefIdAsync(CvAttchmentId, AttachmentRefType.CV, profile.Id);

            }
            else if (Cv is null && CvAttchmentId != 0)
                await _attachmentManager.CheckAndUpdateRefIdAsync(CvAttchmentId, AttachmentRefType.CV, profile.Id);

            profile.LastModificationTime = DateTime.Now;
            await UnitOfWorkManager.Current.SaveChangesAsync();
            var result = MapToEntityDto(profile);
            return result;
        }


        public async Task ToggleActiveStatusAsync(int profileId)
        {
            try
            {
                var profile = await _profileManager.GetEntityByIdWithUserAsync(profileId);
                if (profile == null)
                {
                    throw new EntityNotFoundException($"Profile with ID {profileId} not found.");
                }

                profile.User.IsActive = !profile.User.IsActive;

                await _userManager.UpdateAsync(profile.User);
                await UnitOfWorkManager.Current.SaveChangesAsync();

                if (!profile.User.IsActive)
                    _deactivatedUsersSet.Add(profile.UserId);
                else _deactivatedUsersSet.Remove(profile.UserId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public override async Task<PagedResultDto<ProfileLiteDto>> GetAllAsync(PagedProfileResultRequestDto input)
        {
            var result = await base.GetAllAsync(input);

            /*var attachments = await _attachmentManager.GetListByRefAsync(result.Items.Select(x => (long)x.Id).ToList(), AttachmentRefType.ProfileLogo);

            var attachmentsDict = new Dictionary<long, List<Attachment>>();

            if (attachments.Count > 0)
                attachmentsDict = attachments.GroupBy(A => A.RefId.Value).ToDictionary(G => G.Key, G => G.ToList());

            foreach (var item in result.Items)
            {
                if (attachmentsDict.TryGetValue(item.Id, out var itemAttachments))
                {
                    item.Profile = itemAttachments
                        .Select(A => new LiteAttachmentDto(A.Id, _attachmentManager.GetUrl(A), _attachmentManager.GetLowResolutionPhotoUrl(A), A.Size))
                        .FirstOrDefault();
                }


            }*/
            return result;
        }

        protected override IQueryable<Profile> CreateFilteredQuery(PagedProfileResultRequestDto input)
        {
            var data = base.CreateFilteredQuery(input);
            data = data.Include(x => x.User);
            data = data.Include(x => x.City).ThenInclude(x => x.Translations);
            data = data.Include(x => x.City).ThenInclude(x => x.Country).ThenInclude(x => x.Translations);




            if (!string.IsNullOrEmpty(input.Keyword))
            {
                var keyword = input.Keyword.ToLower();

                data = data.Where(p =>

                    p.User.RegistrationFullName.Contains(keyword) ||
                    p.About.Contains(keyword)
                );
            }

            if (input.CityId.HasValue)
                data = data.Where(x => x.CityId == input.CityId.Value);


            return data;
        }
        protected override IQueryable<Profile> ApplySorting(IQueryable<Profile> query, PagedProfileResultRequestDto input)
        {

            return query.OrderByDescending(r => r.CreationTime);
        }

        private async Task BulkHardDeleteOldEntities(long profileId)
        {
            await _workExperienceRepo.HardDeleteAsync(x => x.ProfileId == profileId);
            await _educationRepo.HardDeleteAsync(x => x.ProfileId == profileId);
            await _awardRepo.HardDeleteAsync(x => x.ProfileId == profileId);
            await _spLanRepo.HardDeleteAsync(x => x.ProfileId == profileId);
        }
    }
}
