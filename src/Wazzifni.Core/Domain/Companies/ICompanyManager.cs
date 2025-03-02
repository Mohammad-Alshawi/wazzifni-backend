using Abp.Domain.Services;
using System.Threading.Tasks;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Domain.Companies
{
    public interface ICompanyManager : IDomainService
    {

        public Task<Company> GetEntityByIdAsync(int id);

        Task<CompanyStatus> GetCompanyStatusByUserIdAsync(long userId);
        Task<int> GetCompanyIdByUserId(long userId);

        Task<Company> GetFullEntityByIdAsync(int id);

        Task<bool> IsUserCompanyOwner(long userId, long companyId);
        Task<Company> GetSuperLiteEntityByIdAsync(int id);

    }
}
