using Abp.Domain.Services;
using System.Threading.Tasks;

namespace Wazzifni.Domain.Companies
{
    public interface ICompanyManager : IDomainService
    {

        public Task<Company> GetEntityByIdAsync(int id);
    }
}
