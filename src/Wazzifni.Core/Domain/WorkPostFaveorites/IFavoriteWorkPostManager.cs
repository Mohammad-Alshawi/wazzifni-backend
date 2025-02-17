using Abp.Domain.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wazzifni.Domain.WorkPosts;

namespace Wazzifni.Domain.WorkPostFaveorites
{
    public interface IFavoriteWorkPostManager : IDomainService
    {
        Task<bool> CheckIfWorkPostInFavoritesAsync(long WorkPostId, long userId);
        Task<int> CountTimesAddedToFavouritesAsync(long WorkPostId);
        Task<bool> DeleteWorkPostFromFavouriteAsync(long WorkPostId, long userId);
        Task AddWorkPostToFavouriteAsync(FavoriteWorkPost input);
        Task DeleteAllFavouritWorkPostsByUserId(long userId);
        Task<int> AdditionsToFavoritesCountForWorkPost(long prpertyId, long userId);

        IQueryable<WorkPost> GetFavoriteWorkPostsQueryByUserIdAsync(long userId);

        Task<HashSet<long>> GetUserFavoriteWorkPostIdsAsync(long userId, List<long> workPostIds);
    }
}
