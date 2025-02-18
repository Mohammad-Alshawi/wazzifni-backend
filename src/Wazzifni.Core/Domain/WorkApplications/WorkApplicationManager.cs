using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wazzifni.Domain.IndividualUserProfiles;
using Wazzifni.Domain.WorkPosts;

namespace Wazzifni.Domain.WorkApplications
{
    public class WorkApplicationManager : DomainService, IWorkApplicationManager
    {
        private readonly IRepository<WorkApplication, long> _repository;
        private readonly IRepository<WorkPost, long> _workPostRepository;
        private readonly IProfileManager _profileManager;

        public WorkApplicationManager(IRepository<WorkApplication, long> repository, IRepository<WorkPost, long> workPostRepository, IProfileManager profileManager)
        {
            _repository = repository;
            _workPostRepository = workPostRepository;
            _profileManager = profileManager;
        }


        public async Task<WorkApplication> GetEntityByIdAsync(long workApplicationId)
        {
            return await _repository
                .GetAll().Include(x => x.Profile).ThenInclude(x => x.User)
                .Include(x => x.WorkPost).ThenInclude(x => x.Company).ThenInclude(x => x.Translations)
                .AsNoTracking().Where(x => x.Id == workApplicationId).FirstOrDefaultAsync();
        }

        public async Task<WorkApplication> GetEntityByIdAsTrackingAsync(long workApplicationId)
        {
            return await _repository
                .GetAll().Include(x => x.Profile).ThenInclude(x => x.User)
                .Include(x => x.WorkPost).ThenInclude(x => x.Company).ThenInclude(x => x.Translations)
                .Where(x => x.Id == workApplicationId).FirstOrDefaultAsync();
        }

        public async Task<HashSet<long>> GetUserAppliedWorkPostIdsAsync(long userId, List<long> workPostIds)
        {
            var profileId = await _profileManager.GetProfileIdByUserId(userId);

            var appliedPostIds = _repository
                    .GetAll()
                    .Where(f => f.ProfileId == profileId && workPostIds.Contains(f.WorkPostId))
                    .Select(f => f.WorkPostId)
                    .ToHashSet();

            return appliedPostIds;
        }

        public IQueryable<WorkPost> GetApplyWorkPostsQueryByUserIdAsync(long userId)
        {
            var profileId = _profileManager.GetProfileIdByUserId(userId).Result;

            var applicationQuery = _repository.GetAll()
                 .Where(F => F.ProfileId == profileId);

            var WorkPostQuery = _workPostRepository.GetAll().Join(
                applicationQuery,
                WorkPost => WorkPost.Id,
                app => app.WorkPostId,
                (WorkPost, app) => WorkPost
            );

            return WorkPostQuery;
        }
    }
}
