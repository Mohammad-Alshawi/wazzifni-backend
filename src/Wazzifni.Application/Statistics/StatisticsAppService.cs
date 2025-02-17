using Abp.Domain.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Wazzifni.Domain.WorkApplications;
using Wazzifni.Statistics.Dto;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Statistics
{
    public class StatisticsAppService : IStatisticsAppService
    {
        private readonly IRepository<WorkApplication, long> _WorkApplicationRepository;
        private readonly IWorkApplicationManager _WorkApplicationManager;
        private readonly IMapper _mapper;

        public StatisticsAppService(
            IRepository<WorkApplication, long> WorkApplicationRepository,
            IWorkApplicationManager WorkApplicationManager,
            IMapper mapper
        )
        {
            _WorkApplicationRepository = WorkApplicationRepository;
            _WorkApplicationManager = WorkApplicationManager;
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
    }
}
