using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Wazzifni.Domain.Courses;
using Wazzifni.Domain.Trainees;

namespace Wazzifni.Domain.CourseRegistrationRequests
{
    public class CourseRegistrationRequestManager : DomainService, ICourseRegistrationRequestManager
    {
        private readonly IRepository<CourseRegistrationRequest, long> _repository;
        private readonly IRepository<Course> _CourseRepository;
        private readonly ITraineeManager _traineeManager;

        public CourseRegistrationRequestManager(IRepository<CourseRegistrationRequest, long> repository, IRepository<Course> CourseRepository, ITraineeManager traineeManager)
        {
            _repository = repository;
            _CourseRepository = CourseRepository;
            _traineeManager = traineeManager;
        }


        public async Task<CourseRegistrationRequest> GetEntityByIdAsync(long CourseRegistrationRequestId)
        {
            return await _repository
                .GetAll().Include(x => x.User).ThenInclude(x => x.Profile)
                .Include(x => x.User).ThenInclude(x => x.Company)
                .Include(x => x.Course).ThenInclude(x => x.Translations)
                .AsNoTracking().Where(x => x.Id == CourseRegistrationRequestId).FirstOrDefaultAsync();
        }

        public async Task<CourseRegistrationRequest> GetEntityByIdAsTrackingAsync(long CourseRegistrationRequestId)
        {
            return await _repository
                .GetAll().Include(x => x.User).ThenInclude(x => x.Profile)
                .Include(x => x.User).ThenInclude(x => x.Company)
                .Include(x => x.Course).ThenInclude(x => x.Translations)
                .Where(x => x.Id == CourseRegistrationRequestId).FirstOrDefaultAsync();
        }

        public async Task<HashSet<int>> GetUserRigestredCoursesIdsAsync(long userId, List<long> coursesIds)
        {
            var traineeId = await _traineeManager.GetTraineeIdByUserId(userId);

            var courseRegistredIds = _repository
                    .GetAll()
                    .Where(f => f.UserId == userId && coursesIds.Contains(f.CourseId))
                    .Select(f => f.CourseId)
                    .ToHashSet();

            return courseRegistredIds;
        }

        public IQueryable<Course> GetApplyCourseQueryByUserIdAsync(long userId)
        {

            var applicationQuery = _repository.GetAll()
                 .Where(F => F.UserId == userId);

            var CourseQuery = _CourseRepository.GetAll().Join(
                applicationQuery,
                Course => Course.Id,
                app => app.CourseId,
                (Course, app) => Course
            );

            return CourseQuery;
        }

        public async Task<bool> CheckIfCourseInRegesrationsUserAsync(long CourseId, long userId)
        {
            //var traineeId = _traineeManager.GetTraineeIdByUserId(userId).Result;

            return await _repository.GetAll().AnyAsync(x => x.CourseId == CourseId && x.UserId == userId);

        }
    }
}
