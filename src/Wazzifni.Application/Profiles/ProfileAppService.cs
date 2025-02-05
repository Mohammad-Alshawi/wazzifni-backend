using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.UI;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Wazzifni.Authorization.Users;
using Wazzifni.Awards;
using Wazzifni.CrudAppServiceBase;
using Wazzifni.Domain.Attachments;
using Wazzifni.Domain.Educations;
using Wazzifni.Domain.IndividualUserProfiles;
using Wazzifni.Domain.Skills;
using Wazzifni.Domain.SpokenLanguages;
using Wazzifni.Domain.WorkExperiences;
using Wazzifni.Profiles.Dto;
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
        private readonly IMapper _mapper;

        public ProfileAppService(IRepository<Profile, long> repository, IAttachmentManager attachmentManager, UserManager userManager,
            IProfileManager profileManager, ISkillManager skillManager,
            IRepository<Skill> skillRepo,
            IRepository<Award, long> awardRepo,
            IRepository<Education, long> educationRepo,
            IRepository<WorkExperience, long> workExperienceRepo,
            IRepository<SpokenLanguageValue, long> spLanRepo,
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
            _mapper = mapper;
        }


        public override async Task<ProfileDetailsDto> GetAsync(EntityDto<long> input)
        {
            var profile = await _profileManager.GetEntityByIdAsync(input.Id);

            return _mapper.Map<ProfileDetailsDto>(profile);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [RemoteService(IsEnabled = false)]
        public override Task<ProfileDetailsDto> CreateAsync(CreateProfileDto input)
        {
            return base.CreateAsync(input);
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [RemoteService(IsEnabled = false)]
        public override Task DeleteAsync(EntityDto<long> input)
        {
            return base.DeleteAsync(input);
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

            await Repository.UpdateAsync(profile);
            UnitOfWorkManager.Current.SaveChanges();

            return _mapper.Map<ProfileDetailsDto>(profile);
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


            if (!input.Keyword.IsNullOrEmpty())
                data = data.Where(x => x.User.FullName.ToString().Contains(input.Keyword));


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
