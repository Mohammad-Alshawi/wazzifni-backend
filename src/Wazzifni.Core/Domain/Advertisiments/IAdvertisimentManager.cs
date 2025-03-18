using Abp.Domain.Services;
using System.Threading.Tasks;

namespace Wazzifni.Domain.Advertisiments
{
    public interface IAdvertisimentManager : IDomainService
    {
        Task<Advertisiment> CheckAdvertisiment(int Id);
        Task<Advertisiment> GetEntityAsync(int Id);
        Task<Advertisiment> InsertAsync(Advertisiment advertisiment);
        Task<bool> RemoveWorkPostFromAdvertisimentByWorkPostId(long WorkPostId);

    }
}
