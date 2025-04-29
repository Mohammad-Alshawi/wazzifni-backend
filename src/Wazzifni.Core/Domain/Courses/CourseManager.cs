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
using Abp.Domain.Entities;
using Wazzifni.Domain.Trainees;
using Wazzifni.Domain.CourseComments;

namespace Wazzifni.Domain.Courses
{
    public class CourseManager : DomainService, ICourseManager
    {
        private readonly IRepository<Course> _CourseRepository;
        private readonly IMapper _mapper;
        private readonly IRepository<CourseComment, long> _courseCommentRepository;
        private readonly IRepository<CourseTranslation> _CourseTranslationsRepository;
        private readonly IRepository<CourseRate, long> _courseRateRepository;
        private readonly IAttachmentManager _attachmentManager;
        private readonly ICityManager _cityManager;
        private readonly ITraineeManager _traineeManager;
        private readonly UserManager _userManager;
        public CourseManager(
            IRepository<CourseTranslation> CourseTranslationsRepository,
            IRepository<CourseRate,long> CourseRateRepository,
            IAttachmentManager attachmentManager,
            ICityManager cityManager,
            ITraineeManager traineeManager,
            IRepository<Course> CourseRepository,
            IMapper mapper,
            IRepository<CourseComment,long> courseCommentRepository,
            UserManager userManager)
        {
            _CourseRepository = CourseRepository;
            _mapper = mapper;
            _courseCommentRepository = courseCommentRepository;
            _CourseTranslationsRepository = CourseTranslationsRepository;
            _courseRateRepository = CourseRateRepository;
            _attachmentManager = attachmentManager;
            _cityManager = cityManager;
            _traineeManager = traineeManager;
            _userManager = userManager;
        }


        public async Task<Course> GetSuperLiteEntityByIdAsync(int id)
        {
            return await _CourseRepository.GetAll().Include(x => x.Translations).Where(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task<Course> GetFullEntityByIdAsync(int id)
        {
            return await _CourseRepository
                .GetAll().Include(x => x.Translations)
                .Include(x => x.City)
                .ThenInclude(x => x.Translations)
                .Include(c => c.Tags).ThenInclude(x=>x.Translations)
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


        public async Task<Course> GetEntityByAsTrackingIdAsync(int id)
        {
            var entity = await _CourseRepository
                  .GetAll().Include(x => x.Translations)
                           .Include(x => x.Tags).ThenInclude(x => x.Translations)
                           .Where(x => x.Id == id)
                           .FirstOrDefaultAsync();
            if (entity == null)
                throw new EntityNotFoundException(typeof(Course), id);
            return entity;
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

        public async Task<CourseRate> RateForCourseByUserId(long userId, int CourseId, double rate)
        {
            var userRate = await _courseRateRepository.FirstOrDefaultAsync(x => x.UserId == userId && x.CourseId == CourseId);

            var traineeId = await _traineeManager.GetTraineeIdByUserId(userId);
            if (userRate is null)
            {
                return await _courseRateRepository.InsertAsync(new CourseRate
                {
                    CourseId = CourseId,
                    UserId = userId,
                    TraineeId = traineeId,
                    Rate = rate
                });
            }
            else
            {
                userRate.Rate = rate;
                return await _courseRateRepository.UpdateAsync(userRate);
            }
        }

        public async Task<double?> GetCourseRateForUser(long userId, int CourseId)
        {
            var userRate = await _courseRateRepository.FirstOrDefaultAsync(x => x.UserId == userId && x.CourseId == CourseId);
            if (userRate is not null)
                return userRate.Rate;
            return null;
        }

        public async Task<double?> GetAverageRatingForCourse(int CourseId)
        {

            var evaluations = await _courseRateRepository.GetAll()
                              .Where(x => x.CourseId == CourseId)
                              .Select(x => x.Rate)
                              .ToListAsync();

            if (evaluations.Any())
                return evaluations.Average();

            return null;
        }

        public async Task DeleteCourseComment(CourseComment comment)
        {
            await _courseCommentRepository.DeleteAsync(comment);
        }

        public async Task<bool> IsCategoryHasCoursesAsync(int courseCategoryId)
        {
            return await _CourseRepository.GetAll()
                .AnyAsync(c => c.CourseCategoryId == courseCategoryId); 
        }

        public async Task<bool> IsTeacherHasCoursesAsync(int teacherId)
        {
            return await _CourseRepository.GetAll()
                .AnyAsync(c => c.TeacherId == teacherId);
        }

    }
}
