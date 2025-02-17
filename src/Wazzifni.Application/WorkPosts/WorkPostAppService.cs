using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Wazzifni.Authorization;
using Wazzifni.Authorization.Users;
using Wazzifni.CrudAppServiceBase;
using Wazzifni.Domain.Companies;
using Wazzifni.Domain.WorkPosts;
using Wazzifni.WorkPosts.Dto;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.WorkPosts
{
    public class WorkPostAppService :
         WazzifniAsyncCrudAppService<WorkPost, WorkPostDetailsDto, long, WorkPostLiteDto, PagedWorkPostResultRequestDto, CreateWorkPostDto, UpdateWorkPostDto>,
         IWorkPostAppService
    {
        private readonly IMapper _mapper;
        private readonly UserManager _userManager;
        private readonly IWorkPostManager _workPostManager;
        private readonly ICompanyManager _companyManager;

        public WorkPostAppService(IRepository<WorkPost, long> repository,
            IMapper mapper, UserManager userManager,
            IWorkPostManager workPostManager,
            ICompanyManager companyManager) : base(repository)
        {
            _mapper = mapper;
            _userManager = userManager;
            _workPostManager = workPostManager;
            _companyManager = companyManager;
        }

        [HttpPost, AbpAuthorize(PermissionNames.WorkPosts_Create)]
        public override async Task<WorkPostDetailsDto> CreateAsync(CreateWorkPostDto input)
        {
            var post = _mapper.Map<WorkPost>(input);

            if (await _userManager.IsCompany())
            {
                var companyId = await _companyManager.GetCompanyIdByUserId(AbpSession.UserId.Value);
                post.CompanyId = companyId;
            }
            else post.CompanyId = input.CompanyId.Value;

            post.Status = WorkPostStatus.Approved;
            post.WorkAvailbility = WorkAvailbility.Available;

            await Repository.InsertAndGetIdAsync(post);
            UnitOfWorkManager.Current.SaveChanges();

            return _mapper.Map<WorkPostDetailsDto>(post);
        }


        [HttpGet]
        public override async Task<WorkPostDetailsDto> GetAsync(EntityDto<long> input)
        {
            var post = await _workPostManager.GetEntityByIdAsync(input.Id);

            return _mapper.Map<WorkPostDetailsDto>(post);
        }



        [HttpPut, AbpAuthorize(PermissionNames.WorkPosts_Update)]


        public override async Task<WorkPostDetailsDto> UpdateAsync(UpdateWorkPostDto input)
        {
            var post = await _workPostManager.GetEntityByIdAsync(input.Id);

            var oldCompanyId = post.CompanyId;

            _mapper.Map(input, post);

            if (await _userManager.IsCompany())
            {
                var companyId = await _companyManager.GetCompanyIdByUserId(AbpSession.UserId.Value);
                if (oldCompanyId != companyId)
                {
                    throw new UserFriendlyException("Denied");
                }
                post.CompanyId = companyId;
            }
            else
            {
                post.CompanyId = input.CompanyId.Value;
            }
            await Repository.UpdateAsync(post);
            await UnitOfWorkManager.Current.SaveChangesAsync();
            return _mapper.Map<WorkPostDetailsDto>(post);
        }



        [AbpAllowAnonymous]
        public override async Task<PagedResultDto<WorkPostLiteDto>> GetAllAsync(PagedWorkPostResultRequestDto input)
        {
            var result = await base.GetAllAsync(input);
            return result;
        }

        protected override IQueryable<WorkPost> CreateFilteredQuery(PagedWorkPostResultRequestDto input)
        {
            var data = base.CreateFilteredQuery(input);
            data = data.Include(x => x.Company).ThenInclude(x => x.User);
            data = data.Include(x => x.Company).ThenInclude(x => x.Translations);
            data = data.Include(x => x.Company).ThenInclude(x => x.City).ThenInclude(x => x.Translations);
            data = data.Include(x => x.Applications);

            if (!string.IsNullOrEmpty(input.Keyword))
            {
                data = data.Where(wp =>
                    wp.Title.Contains(input.Keyword) ||
                    wp.Description.Contains(input.Keyword) ||
                    wp.EducationLevel.ToString().Contains(input.Keyword) ||
                    wp.WorkEngagement.ToString().Contains(input.Keyword) ||
                    wp.WorkLevel.ToString().Contains(input.Keyword) ||
                    wp.Company.Translations.Any(t => t.Name.Contains(input.Keyword)) ||
                    wp.Company.City.Translations.Any(t => t.Name.Contains(input.Keyword))
                );
            }

            if (input.CompanyId.HasValue)
                data = data.Where(wp => wp.CompanyId == input.CompanyId.Value);

            if (input.Status.HasValue)
                data = data.Where(wp => wp.Status == input.Status.Value);

            if (input.WorkEngagement.HasValue)
                data = data.Where(wp => wp.WorkEngagement == input.WorkEngagement.Value);

            if (input.WorkLevel.HasValue)
                data = data.Where(wp => wp.WorkLevel == input.WorkLevel.Value);

            if (input.EducationLevel.HasValue)
                data = data.Where(wp => wp.EducationLevel == input.EducationLevel.Value);

            if (input.MinSalary.HasValue)
                data = data.Where(wp => wp.MinSalary >= input.MinSalary.Value);

            if (input.MaxSalary.HasValue)
                data = data.Where(wp => wp.MaxSalary <= input.MaxSalary.Value);

            if (input.WorkAvailbility.HasValue)
                data = data.Where(wp => wp.WorkAvailbility == input.WorkAvailbility.Value);

            if (input.ProfileId.HasValue)
                data = data.Where(wp => wp.Applications.Any(x => x.ProfileId == input.ProfileId.Value));


            return data;
        }
        protected override IQueryable<WorkPost> ApplySorting(IQueryable<WorkPost> query, PagedWorkPostResultRequestDto input)
        {

            return query.OrderByDescending(r => r.CreationTime);
        }
    }
}
