using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Services;

namespace Wazzifni.Domain.Courses
{
    public interface ICourseManager : IDomainService
    {
        Task<Course> GetSuperLiteEntityByIdAsync(int id);
        Task<Course> GetFullEntityByIdAsync(int id);
        Task<Course> GetEntityByIdAsync(int id);
        Task<Course> GetLiteCourseByIdAsync(int id);
        Task<Course> GetEntityByAsTrackingIdAsync(int id);
        Task<CourseRate> RateForCourseByUserId(long userId, int CourseId, double rate);
        Task<double?> GetAverageRatingForCourse(int CourseId);
        Task<double?> GetCourseRateForUser(long userId, int CourseId);
    }
}
