using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Wazzifni.Authorization.Users;
using Wazzifni.Domain.Attachments;
using Wazzifni.Domain.Trainees;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Domain.Trainees
{
    public class TraineeManager : DomainService, ITraineeManager
    {
        private readonly IRepository<Trainee, long> repository;
        private readonly UserManager userManager;
        private readonly IAttachmentManager attachmentManager;

        public TraineeManager(IRepository<Trainee, long> repository, UserManager userManager , IAttachmentManager attachmentManager)
        {
            this.repository = repository;
            this.userManager = userManager;
            this.attachmentManager = attachmentManager;
        }

        public async Task<Trainee> GetEntityByIdAsync(long TraineeId)
        {
            return await repository
                .GetAllIncluding(x => x.User)
                .Include(x => x.University)
                .ThenInclude(x => x.Translations)
                .AsNoTracking().Where(x => x.Id == TraineeId).FirstOrDefaultAsync();
        }

        public async Task<Trainee> GetEntityByUserIdAsync(long UserId)
        {
            return await repository
              .GetAllIncluding(x => x.User)
              .Include(x => x.University)
               .ThenInclude(x => x.Translations)
              .Where(x => x.UserId == UserId).FirstOrDefaultAsync();
        }

        public async Task<long> InitateTrainee(long UserId, int? UniversityId, string? UniversityMajor ,long LogoAttchmentId)
        {
            var Trainee = new Trainee { UserId = UserId, UniversityId = UniversityId , UniversityMajor = UniversityMajor };

            var id = await repository.InsertAndGetIdAsync(Trainee);

            if ( LogoAttchmentId != 0)
            {
                await attachmentManager.CheckAndUpdateRefIdAsync(LogoAttchmentId, AttachmentRefType.Trainee, id);
            }
          

            return id;  

        }

        public async Task<long> GetTraineeIdByUserId(long userId)
        {
            return await repository.GetAll().Where(x => x.UserId == userId).Select(x => x.Id).FirstOrDefaultAsync();
        }

        public async Task DeleteTraineeByUserId(long userId)
        {
            var Trainee = await repository.GetAll()
                           .Include(x => x.User)
                           .Include(x => x.University)
                           .ThenInclude(x => x.Translations)
                           .Where(x => x.UserId == userId).FirstOrDefaultAsync();

            await repository.DeleteAsync(Trainee);
        }

        public async Task<Trainee> GetEntityByIdWithUserAsync(long TraineeId)
        {
            return await repository
                .GetAllIncluding(x => x.User).Where(x => x.Id == TraineeId).FirstOrDefaultAsync();
        }
    }
}
