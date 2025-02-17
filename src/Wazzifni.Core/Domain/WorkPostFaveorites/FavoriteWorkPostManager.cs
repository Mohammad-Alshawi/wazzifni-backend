using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wazzifni.Domain.WorkPosts;

namespace Wazzifni.Domain.WorkPostFaveorites
{
    public class FavoriteWorkPostManager : IFavoriteWorkPostManager
    {
        private readonly IRepository<WorkPost, long> _WorkPostRepository;
        private readonly IRepository<FavoriteWorkPost, long> _favoriteWorkPostRepository;

        public FavoriteWorkPostManager(
            IRepository<WorkPost, long> WorkPostRepository,
            IRepository<FavoriteWorkPost, long> favoriteWorkPostRepository
        )
        {
            _WorkPostRepository = WorkPostRepository;
            _favoriteWorkPostRepository = favoriteWorkPostRepository;
        }

        public async Task<bool> CheckIfWorkPostInFavoritesAsync(long WorkPostId, long userId)
        {
            return await _favoriteWorkPostRepository.GetAll().AnyAsync(x => x.WorkPostId == WorkPostId && x.CreatorUserId == userId);
        }

        public async Task<bool> DeleteWorkPostFromFavouriteAsync(long WorkPostId, long userId)
        {
            var favoriteWorkPost = await _favoriteWorkPostRepository.GetAll().FirstOrDefaultAsync(x => x.WorkPostId == WorkPostId && x.CreatorUserId == userId);
            if (favoriteWorkPost is not null)
            {
                await _favoriteWorkPostRepository.HardDeleteAsync(favoriteWorkPost);
                return true;
            }
            return false;
        }

        public async Task<int> CountTimesAddedToFavouritesAsync(long WorkPostId)
        {
            return await _favoriteWorkPostRepository.GetAll().CountAsync(x => x.WorkPostId == WorkPostId);

        }

        public async Task AddWorkPostToFavouriteAsync(FavoriteWorkPost input)
        {
            await _favoriteWorkPostRepository.InsertAsync(input);
        }

        public IQueryable<WorkPost> GetFavoriteWorkPostsQueryByUserIdAsync(long userId)
        {
            var favoriteQuery = _favoriteWorkPostRepository.GetAll()
                .Where(F => F.CreatorUserId == userId);

            var WorkPostQuery = _WorkPostRepository.GetAll().Join(
                favoriteQuery,
                WorkPost => WorkPost.Id,
                favorite => favorite.WorkPostId,
                (WorkPost, favorite) => WorkPost
            );

            return WorkPostQuery;
        }

        public async Task DeleteAllFavouritWorkPostsByUserId(long userId)
        {
            var properrties = await _favoriteWorkPostRepository.GetAll().Where(x => x.CreatorUserId == userId).ToListAsync();
            properrties.ForEach(async prop =>
            await _favoriteWorkPostRepository.DeleteAsync(prop));
        }

        public async Task<int> AdditionsToFavoritesCountForWorkPost(long prpertyId, long userId)
        {
            return await _favoriteWorkPostRepository.GetAll().AsNoTracking().CountAsync(x => x.WorkPostId == prpertyId && x.CreatorUserId != userId);
        }

        public async Task<HashSet<long>> GetUserFavoriteWorkPostIdsAsync(long userId, List<long> workPostIds)
        {
            var favoritePostIds = _favoriteWorkPostRepository
                    .GetAll()
                    .Where(f => f.CreatorUserId == userId && workPostIds.Contains(f.WorkPostId))
                    .Select(f => f.WorkPostId)
                    .ToHashSet();
            return favoritePostIds;
        }

    }
}
