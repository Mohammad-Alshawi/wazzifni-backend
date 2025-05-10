using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.UI;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Wazzifni.Authorization;
using Wazzifni.Authorization.Users;
using Wazzifni.Awards;
using Wazzifni.CrudAppServiceBase;
using Wazzifni.Domain.Attachments;
using Wazzifni.Domain.Companies;
using Wazzifni.Domain.Educations;
using Wazzifni.Domain.IndividualUserProfiles;
using Wazzifni.Domain.Skills;
using Wazzifni.Domain.SpokenLanguages;
using Wazzifni.Domain.Trainees;
using Wazzifni.Domain.WorkExperiences;
using Wazzifni.Trainees.Dto;
using static Wazzifni.Enums.Enum;


namespace Wazzifni.Trainees
{
    public class TraineeAppService :
         WazzifniAsyncCrudAppService<Trainee, TraineeDetailsDto, long, TraineeLiteDto, PagedTraineeResultRequestDto, CreateTraineeDto, UpdateTraineeDto>,
         ITraineeAppService
    {
        private readonly IRepository<Trainee, long> _repository;
        private readonly IAttachmentManager _attachmentManager;
        private readonly UserManager _userManager;
        private readonly ITraineeManager _TraineeManager;
        private readonly DeactivatedUsersSet _deactivatedUsersSet;
        private readonly IMapper _mapper;

        public TraineeAppService(IRepository<Trainee, long> repository, IAttachmentManager attachmentManager, UserManager userManager,
            ITraineeManager TraineeManager, 
           
            DeactivatedUsersSet deactivatedUsersSet,
            IMapper mapper) : base(repository)
        {
            _repository = repository;
            _attachmentManager = attachmentManager;
            _userManager = userManager;
            _TraineeManager = TraineeManager;
            _deactivatedUsersSet = deactivatedUsersSet;
            _mapper = mapper;
        }


        public override async Task<TraineeDetailsDto> GetAsync(EntityDto<long> input)
        {
            var Trainee = await _TraineeManager.GetEntityByIdAsync(input.Id);

            var result = _mapper.Map<TraineeDetailsDto>(Trainee);
            var logo = await _attachmentManager.GetElementByRefAsync(result.Id, AttachmentRefType.Trainee);
            if (logo is not null)
            {
                result.Image = new LiteAttachmentDto
                {
                    Id = logo.Id,
                    Url = _attachmentManager.GetUrl(logo),
                    LowResolutionPhotoUrl = _attachmentManager.GetLowResolutionPhotoUrl(logo),
                };
            }

            return result;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [RemoteService(IsEnabled = false)]
        public override Task<TraineeDetailsDto> CreateAsync(CreateTraineeDto input)
        {
            return base.CreateAsync(input);
        }


      
        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var trainee = await Repository.GetAllIncluding(
                           c => c.User,
                           c => c.CourseRates,
                           c => c.CourseRegistrationRequests,
                           c => c.CourseComments
                       ).FirstOrDefaultAsync(c => c.Id == input.Id);

            if (trainee == null)
            {
                throw new UserFriendlyException("Not Found");
            }

            trainee.CourseRates.Clear();
            trainee.CourseRegistrationRequests.Clear();
            trainee.CourseComments.Clear();

            await Repository.DeleteAsync(trainee);

            var user = await _userManager.Users
              .Where(t => t.Id == trainee.UserId)
              .FirstOrDefaultAsync();

            await _userManager.DeleteAsync(user);

            await UnitOfWorkManager.Current.SaveChangesAsync();
        }


        [AbpAuthorize]
        public override async Task<TraineeDetailsDto> UpdateAsync(UpdateTraineeDto input)
        {
            var Trainee = await _TraineeManager.GetEntityByUserIdAsync(AbpSession.UserId.Value);

            if (Trainee.Id != input.Id || !await _userManager.IsTrainee())
            {
                throw new UserFriendlyException("Cant do this");
            }

            Trainee = _mapper.Map(input, Trainee);


            var oldAttachment = await _attachmentManager.GetElementByRefAsync(Trainee.Id, AttachmentRefType.Trainee);

            if (input.TraineePhotoId == 0 && oldAttachment != null)
            {
                await _attachmentManager.DeleteRefIdAsync(oldAttachment);
            }
            else if (input.TraineePhotoId != 0 && oldAttachment is not null)
            {
                if (oldAttachment.Id != input.TraineePhotoId)
                {
                    await _attachmentManager.DeleteRefIdAsync(oldAttachment);
                    await _attachmentManager.CheckAndUpdateRefIdAsync(
                     input.TraineePhotoId, AttachmentRefType.Trainee, Trainee.Id);
                }
            }
            else if (input.TraineePhotoId != 0)
            {
                await _attachmentManager.CheckAndUpdateRefIdAsync(input.TraineePhotoId, AttachmentRefType.Trainee, Trainee.Id);
            }
            await Repository.UpdateAsync(Trainee);

            Trainee.User.RegistrationFullName = input.RegistrationFullName;
            Trainee.User.EmailAddress = input.EmailAddress;

            await _userManager.UpdateAsync(Trainee.User);

            UnitOfWorkManager.Current.SaveChanges();

            return _mapper.Map<TraineeDetailsDto>(Trainee);
        }


        [AbpAuthorize]
        public async Task<TraineeDetailsDto> UpdateLogoAsync(int Id, [Required] long LogoAttchmentId)
        {
            if (!await _userManager.IsTrainee())
            {
                throw new UserFriendlyException("Cant do this");
            }
            var Trainee = await _TraineeManager.GetEntityByUserIdAsync(AbpSession.UserId.Value);

            var logo = await _attachmentManager.GetElementByRefAsync(Id, AttachmentRefType.Trainee);
            if (logo is not null && logo.Id != LogoAttchmentId && LogoAttchmentId != 0)
            {
                await _attachmentManager.DeleteRefIdAsync(logo);
                await _attachmentManager.CheckAndUpdateRefIdAsync(LogoAttchmentId, AttachmentRefType.Trainee, Trainee.Id);

            }
            else if (logo is null && LogoAttchmentId != 0)
                await _attachmentManager.CheckAndUpdateRefIdAsync(LogoAttchmentId, AttachmentRefType.Trainee, Trainee.Id);

            Trainee.LastModificationTime = DateTime.Now;
            await UnitOfWorkManager.Current.SaveChangesAsync();
            var result = MapToEntityDto(Trainee);
            return result;
        }



        public async Task ToggleActiveStatusAsync(int TraineeId)
        {
            try
            {
                var Trainee = await _TraineeManager.GetEntityByIdWithUserAsync(TraineeId);
                if (Trainee == null)
                {
                    throw new EntityNotFoundException($"Trainee with ID {TraineeId} not found.");
                }

                Trainee.User.IsActive = !Trainee.User.IsActive;

                await _userManager.UpdateAsync(Trainee.User);
                await UnitOfWorkManager.Current.SaveChangesAsync();

                if (!Trainee.User.IsActive)
                    _deactivatedUsersSet.Add(Trainee.UserId);
                else _deactivatedUsersSet.Remove(Trainee.UserId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public override async Task<PagedResultDto<TraineeLiteDto>> GetAllAsync(PagedTraineeResultRequestDto input)
        {
            var result = await base.GetAllAsync(input);


            var attachments = await _attachmentManager.GetListByRefAsync(result.Items.Select(x => (long)x.Id).ToList(), AttachmentRefType.Trainee);

            var attachmentsDict = new Dictionary<long, List<Attachment>>();

            if (attachments.Count > 0)
                attachmentsDict = attachments.GroupBy(A => A.RefId.Value).ToDictionary(G => G.Key, G => G.ToList());

            foreach (var item in result.Items)
            {
                if (attachmentsDict.TryGetValue(item.Id, out var itemAttachments))
                {
                    item.Image = itemAttachments
                        .Select(A => new LiteAttachmentDto(A.Id, _attachmentManager.GetUrl(A), _attachmentManager.GetLowResolutionPhotoUrl(A), A.Size))
                        .FirstOrDefault();
                }


            }
            return result;
        }

        protected override IQueryable<Trainee> CreateFilteredQuery(PagedTraineeResultRequestDto input)
        {
            var data = base.CreateFilteredQuery(input);
            data = data.Include(x => x.User);
            data = data.Include(x => x.University).ThenInclude(x => x.Translations);




            if (!string.IsNullOrEmpty(input.Keyword))
            {
                var keyword = input.Keyword.ToLower();

                data = data.Where(p =>

                    p.User.RegistrationFullName.Contains(keyword) ||
                    p.UniversityMajor.Contains(keyword) ||
                    p.University.Translations.Any(t => t.Name.Contains(keyword)) 
                );
            }

            if (input.UniversityId.HasValue)
                data = data.Where(x => x.UniversityId == input.UniversityId.Value);


            return data;
        }
        protected override IQueryable<Trainee> ApplySorting(IQueryable<Trainee> query, PagedTraineeResultRequestDto input)
        {

            return query.OrderByDescending(r => r.CreationTime);
        }

     
    }
}
