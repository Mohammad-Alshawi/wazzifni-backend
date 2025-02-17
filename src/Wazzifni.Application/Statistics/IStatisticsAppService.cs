using Abp.Application.Services;
using System.Threading.Tasks;
using Wazzifni.Statistics.Dto;

namespace Wazzifni.Statistics;

public interface IStatisticsAppService : IApplicationService
{
    Task<ApplicationStatsDto> GetWorkApplicationStats(long profileId);

}
