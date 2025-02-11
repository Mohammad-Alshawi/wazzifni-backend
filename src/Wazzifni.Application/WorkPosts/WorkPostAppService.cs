using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly ICompanyManager _companyManager;

        public WorkPostAppService(IRepository<WorkPost, long> repository, IMapper mapper, UserManager userManager, ICompanyManager companyManager) : base(repository)
        {
            _mapper = mapper;
            _userManager = userManager;
            _companyManager = companyManager;
        }

        [HttpPost]
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
            post.WorkVisibility = WorkVisibility.Visible;

            await Repository.InsertAndGetIdAsync(post);
            UnitOfWorkManager.Current.SaveChanges();

            return _mapper.Map<WorkPostDetailsDto>(post);
        }

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

            if (input.WorkVisibility.HasValue)
                data = data.Where(wp => wp.WorkVisibility == input.WorkVisibility.Value);


            return data;
        }
        protected override IQueryable<WorkPost> ApplySorting(IQueryable<WorkPost> query, PagedWorkPostResultRequestDto input)
        {

            return query.OrderByDescending(r => r.CreationTime);
        }
    }
}
