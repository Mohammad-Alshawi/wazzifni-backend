using System.Threading.Tasks;
using Abp.Application.Services;
using Wazzifni.Sessions.Dto;

namespace Wazzifni.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
