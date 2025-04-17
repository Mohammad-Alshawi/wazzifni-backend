using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.UI;
using AutoMapper;
using Wazzifni.Authorization.Users;
using Wazzifni.Domain.Attachments;
using Wazzifni.Domain.Cities;
using Wazzifni.Domain.Companies;
using static Wazzifni.Enums.Enum;
using Wazzifni.Localization.SourceFiles;
using Microsoft.EntityFrameworkCore;

namespace Wazzifni.Domain.Courses
{
    public class CourseManager : DomainService, ICourseManager
    {
        private readonly IRepository<Course> _CourseRepository;
        private readonly IMapper _mapper;

        private readonly IRepository<CourseTranslation> _CourseTranslationsRepository;
        private readonly IAttachmentManager _attachmentManager;
        private readonly ICityManager _cityManager;

        private readonly UserManager _userManager;
        public CourseManager(
            IRepository<CourseTranslation> CourseTranslationsRepository,
            IAttachmentManager attachmentManager,
            ICityManager cityManager,
            IRepository<Course> CourseRepository,
            IMapper mapper,
            UserManager userManager)
        {
            _CourseRepository = CourseRepository;
            _mapper = mapper;
            _CourseTranslationsRepository = CourseTranslationsRepository;
            _attachmentManager = attachmentManager;
            _cityManager = cityManager;
            _userManager = userManager;
        }


        public async Task<Course> GetSuperLiteEntityByIdAsync(int id)
        {
            return await _CourseRepository.GetAll().Include(x => x.Translations).Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task<Course> GetFullEntityByIdAsync(int id)
        {
            return await _CourseRepository
                .GetAllIncluding(x => x.Translations)
                .Include(x => x.City)
                .ThenInclude(x => x.Translations)
                .Include(c => c.Tags)
                .Include(x=>x.CourseCategory).ThenInclude(x=>x.Translations)
                .Include(x=>x.Teacher)
                .AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task<Course> GetEntityByIdAsync(int id)
        {
            return await _CourseRepository.GetAll()
                .AsNoTrackingWithIdentityResolution()
                .Include(x => x.Translations)
                .Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task<Course> GetLiteCourseByIdAsync(int id)
        {
            return await _CourseRepository
                .GetAllIncluding(x => x.Translations)

                .AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
        }








        public async Task<List<Course>> GetListOfCourse(List<long> CourseIds)
        {
            return await _CourseRepository.GetAll().Where(x => CourseIds.Contains(x.Id)).ToListAsync();
        }

   

        public async Task<bool> CheckIfCourseExict(int CourseId)
        {
            if (!await _CourseRepository.GetAll().AnyAsync(x => x.Id == CourseId))
            {
                throw new UserFriendlyException(404, Exceptions.ObjectWasNotFound, "Course" + " " + CourseId.ToString());

            }
            return true;
        }
        public async Task UpdateAttachmentTypeListAsync(List<long> newAttachmentIds, AttachmentRefType attachmentType, long CourseId)
        {
            var existingAttachments = await _attachmentManager.GetByRefAsync(CourseId, attachmentType);
            var imagesattachmentsToDelete = existingAttachments.Where(x => !newAttachmentIds.Contains((x.Id)));
            var imagesattachmentIdsToAdd = newAttachmentIds.Except(existingAttachments.Select(x => x.Id).ToList());
            foreach (var existingAttachment in imagesattachmentsToDelete)
            {
                await _attachmentManager.DeleteRefIdAsync(existingAttachment);
            }

            foreach (var newAttachmentId in imagesattachmentIdsToAdd)
            {
                await _attachmentManager.CheckAndUpdateRefIdAsync(
                    newAttachmentId, attachmentType, CourseId);
            }
        }


        public async Task HardDeleteCourseTranslation(List<CourseTranslation> translations)
        {
            try
            {
                foreach (var translation in translations)
                {

                    await _CourseTranslationsRepository.HardDeleteAsync(translation);
                }
            }
            catch (Exception ex) { throw; }
        }


        public async Task<int> GetCompaniesCount()
        {
            return await _CourseRepository.GetAll().AsNoTracking().Where(x => x.IsDeleted == false).CountAsync();
        }
    }
}
