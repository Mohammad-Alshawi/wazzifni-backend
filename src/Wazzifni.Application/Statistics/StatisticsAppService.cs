using Abp.Domain.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wazzifni.Domain.Companies;
using Wazzifni.Domain.WorkApplications;
using Wazzifni.Domain.WorkPosts;
using Wazzifni.Statistics.Dto;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Statistics
{
    public class StatisticsAppService : IStatisticsAppService
    {
        private readonly IRepository<WorkApplication, long> _WorkApplicationRepository;
        private readonly IWorkApplicationManager _WorkApplicationManager;
        private readonly IRepository<WorkPost, long> _workPostRepository;
        private readonly IRepository<Company> _companyRepository;
        private readonly IRepository<Domain.IndividualUserProfiles.Profile, long> _profileRepository;
        private readonly IMapper _mapper;

        public StatisticsAppService(
            IRepository<WorkApplication, long> WorkApplicationRepository,
            IWorkApplicationManager WorkApplicationManager,
            IRepository<WorkPost, long> workPostRepository,
            IRepository<Company> companyRepository,
            IRepository<Domain.IndividualUserProfiles.Profile, long> profileRepository,
            IMapper mapper
        )
        {
            _WorkApplicationRepository = WorkApplicationRepository;
            _WorkApplicationManager = WorkApplicationManager;
            _workPostRepository = workPostRepository;
            _companyRepository = companyRepository;
            _profileRepository = profileRepository;
            _mapper = mapper;
        }

        public async Task<ApplicationStatsDto> GetWorkApplicationStats(long profileId)
        {
            var query = _WorkApplicationRepository.GetAll().Where(w => w.ProfileId == profileId);

            var totalApplications = await query.CountAsync();
            var approvedApplications = await query.CountAsync(w => w.Status == WorkApplicationStatus.Approved);

            return new ApplicationStatsDto
            {
                TotalApplications = totalApplications,
                ApprovedApplications = approvedApplications
            };
        }
        public async Task<WorkPostForCompanyStatsDto> GetWorkPostForCompanyStats(int companyId)
        {
            var query = _workPostRepository.GetAll().Where(w => w.CompanyId == companyId);

            var workPostNotHaveApplication = await query.CountAsync(w => w.ApplicantsCount < 1);
            var workPostCountHaveApplication = await query.CountAsync(w => w.ApplicantsCount > 0 && !w.IsClosed);
            var workPostCountIsClosed = await query.CountAsync(w => w.IsClosed);

            return new WorkPostForCompanyStatsDto
            {
                WorkPostNotHaveApplication = workPostNotHaveApplication,
                WorkPostCountHaveApplication = workPostCountHaveApplication,
                WorkPostIsClosed = workPostCountIsClosed
            };
        }

        public async Task<StatisticalNumbersDto> GetStatisticalNumbers()
        {
            var today = DateTime.Today;

            var workPosts = _workPostRepository.GetAll();
            var workApplications = _WorkApplicationRepository.GetAll();
            var companies = _companyRepository.GetAll();
            var profiles = _profileRepository.GetAll();

            var stats = new StatisticalNumbersDto
            {
                WorkPostAll = await workPosts.CountAsync(),
                WorkPostToday = await workPosts.CountAsync(p => p.CreationTime.Date == today),
                WorkApplicationAll = await workApplications.CountAsync(),
                WorkApplicationToday = await workApplications.CountAsync(p => p.CreationTime.Date == today),
                CompanyAll = await companies.CountAsync(),
                CompanyToday = await companies.CountAsync(p => p.CreationTime.Date == today),
                ProfileAll = await profiles.CountAsync(),
                ProfileToday = await profiles.CountAsync(p => p.CreationTime.Date == today)
            };

            return stats;
        }



        public async Task<List<WorkApplicationsChartDto>> GetWorkApplicationsChart(int? month = null, int? year = null)
        {
            var today = DateTime.Today;
            int targetMonth = month ?? today.Month;
            int targetYear = year ?? today.Year;

            var startOfMonth = new DateTime(targetYear, targetMonth, 1);
            var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

            var applications = await _WorkApplicationRepository
                .GetAll()
                .Where(a => a.CreationTime >= startOfMonth && a.CreationTime <= endOfMonth)
                .GroupBy(a => a.CreationTime.Date)
                .Select(g => new WorkApplicationsChartDto
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderBy(a => a.Date)
                .ToListAsync();

            return applications;
        }



    }
}
