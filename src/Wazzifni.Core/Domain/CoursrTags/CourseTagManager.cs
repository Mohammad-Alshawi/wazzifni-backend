using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.UI;
using Wazzifni.Domain.CourseTags;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wazzifni.Domain.Courses;

namespace Wazzifni.Domain.CourseTags
{
    public class CourseTagManager: ICourseTagManager
    {
        private readonly IRepository<CourseTag> _CourseTagRepository;
        private readonly IRepository<CourseTagTranslation> _CourseTagTranslationRepository;
        private readonly IRepository<Course> _CourseRepository;

        public CourseTagManager(IRepository<CourseTag> CourseTagRepository, IRepository<CourseTagTranslation> CourseTagTranslationRepository, IRepository<Course> CourseRepository)
        {
            _CourseTagRepository = CourseTagRepository;
            _CourseTagTranslationRepository = CourseTagTranslationRepository;
            _CourseRepository = CourseRepository;   
        }
        public async Task<bool> CheckIfCourseTagIsExist(List<CourseTagTranslation> Translations)
        {
            var postCategories = await _CourseTagTranslationRepository.GetAll().ToListAsync();
            foreach (var Translation in Translations)
            {
                foreach (var CourseTag in postCategories)
                    if (CourseTag.Name == Translation.Name && CourseTag.Language == Translation.Language)
                        return true;
            }
            return false;
        }

        public async Task<CourseTag> GetEntityByIdAsync(int id)
        {
            var entity = await _CourseTagRepository.GetAll()
              .Include(c => c.Translations)
              .FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
                throw new EntityNotFoundException(typeof(CourseTag), id);
            return entity;
        }

        public async Task<CourseTag> GetLiteEntityByIdAsync(int id)
        {
            var entity = await _CourseTagRepository.GetAll().Where(x => x.Id == id).Include(x => x.Translations).FirstOrDefaultAsync();
            if (entity == null)
                throw new EntityNotFoundException(typeof(CourseTag), id);
            return entity;
        }
        public async Task<List<string>> GetAllCourseTagNameForAutoComplete(string keyword, int CourseTagId)
        {
            return await _CourseTagTranslationRepository.GetAll().Where(x => x.Name.Contains(keyword) && x.CoreId == CourseTagId).Select(x => x.Name).ToListAsync();
        }

        public async Task CheckIfCourseTagValueCorrect(List<int> CourseTagIds)
        {
            var existingIds = await _CourseTagRepository
                .GetAll()
                .Where(x => CourseTagIds.Contains(x.Id))
                .Select(x => x.Id)
                .ToListAsync();

            var missingIds = CourseTagIds.Except(existingIds).ToList();

            if (missingIds.Any())
            {
                string missingIdsString = string.Join(", ", missingIds);
                throw new UserFriendlyException($"CourseTags With IDs not found : {missingIdsString}");
            }
        }
        public async Task UpdateCourseTag(CourseTag CourseTag)
        {
            await _CourseTagRepository.UpdateAsync(CourseTag);
        }
        public async Task<List<int>> GeCoursesIdsHaveCourseTags(List<int> CourseTagIds)
        {
            var CoursesIds = await _CourseRepository.GetAll()
              .Where(v => v.Tags.Any(t => CourseTagIds.Contains(t.Id)))
              .Select(v => v.Id)
              .ToListAsync();

            return CoursesIds;

        }


    }
}
