using Abp.Domain.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wazzifni.Domain.Courses;
using Wazzifni.Domain.WorkPosts;

namespace Wazzifni.Domain.CourseRegistrationRequests
{
    public interface ICourseRegistrationRequestManager : IDomainService
    {

        Task<CourseRegistrationRequest> GetEntityByIdAsync(long CourseRegistrationRequestId);
        Task<CourseRegistrationRequest> GetEntityByIdAsTrackingAsync(long CourseRegistrationRequestId);
        Task<HashSet<int>> GetUserRigestredCoursesIdsAsync(long userId, List<long> coursesIds);
        IQueryable<Course> GetApplyCourseQueryByUserIdAsync(long userId);
        Task<bool> CheckIfCourseInRegesrationsUserAsync(long CourseId, long userId);

    }
}
