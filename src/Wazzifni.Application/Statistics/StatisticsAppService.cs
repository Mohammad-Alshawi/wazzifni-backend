using Abp.Domain.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly IMapper _mapper;

        public StatisticsAppService(
            IRepository<WorkApplication, long> WorkApplicationRepository,
            IWorkApplicationManager WorkApplicationManager,
            IRepository<WorkPost, long> workPostRepository,
            IMapper mapper
        )
        {
            _WorkApplicationRepository = WorkApplicationRepository;
            _WorkApplicationManager = WorkApplicationManager;
            _workPostRepository = workPostRepository;
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

            var workPostNotHaveApplication = await query.CountAsync(w => w.ApplicantsCount < 0);
            var workPostCountHaveApplication = await query.CountAsync(w => w.ApplicantsCount > 0);
            var workPostCountIsClosed = await query.CountAsync(w => w.IsClosed);

            return new WorkPostForCompanyStatsDto
            {
                WorkPostNotHaveApplication = workPostNotHaveApplication,
                WorkPostCountHaveApplication = workPostCountHaveApplication,
                WorkPostIsClosed = workPostCountIsClosed
            };
        }

    }
}
