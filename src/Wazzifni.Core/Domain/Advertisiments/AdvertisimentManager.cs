using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Wazzifni.Domain.Attachments;

namespace Wazzifni.Domain.Advertisiments
{
    public class AdvertisimentManager : DomainService, IAdvertisimentManager
    {
        private readonly IRepository<Advertisiment> _advertisimentrepository;
        private readonly IAttachmentManager _attachmentManager;
        public AdvertisimentManager(
            IRepository<Advertisiment> advertisimentRepository,
            IAttachmentManager attachmentManager
            )
        {
            _advertisimentrepository = advertisimentRepository;
            _attachmentManager = attachmentManager;

        }


        public async Task<Advertisiment> CheckAdvertisiment(int Id)
        {
            return await _advertisimentrepository.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<Advertisiment> GetEntityAsync(int Id)
        {
            return await _advertisimentrepository.GetAll().FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<Advertisiment> InsertAsync(Advertisiment advertisiment)
        {
            return await _advertisimentrepository.InsertAsync(advertisiment);
        }



        public async Task<bool> RemoveWorkPostFromAdvertisimentByWorkPostId(long WorkPostId)
        {
            var advertisiment = await _advertisimentrepository.GetAll().Where(x => x.WorkPostId == WorkPostId && x.IsDeleted == false).ToListAsync();
            foreach (var item in advertisiment)
            {
                item.WorkPostId = null;
                await _advertisimentrepository.UpdateAsync(item);
            }
            return true;
        }
    }
}

