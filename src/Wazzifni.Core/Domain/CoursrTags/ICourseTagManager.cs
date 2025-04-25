using Abp.Domain.Services;
using Wazzifni.Domain.CourseTags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wazzifni.Domain.CourseTags
{
    public interface ICourseTagManager: IDomainService
    {
        Task<CourseTag> GetEntityByIdAsync(int id);
        Task<bool> CheckIfCourseTagIsExist(List<CourseTagTranslation> Translations);
        Task<CourseTag> GetLiteEntityByIdAsync(int id);
        Task<List<string>> GetAllCourseTagNameForAutoComplete(string keyword, int CourseTagId);
        Task CheckIfCourseTagValueCorrect(List<int> CourseTagIds);
        Task UpdateCourseTag(CourseTag CourseTag);
        Task<List<int>> GeCoursesIdsHaveCourseTags(List<int> CourseTagIds);


    }
}
