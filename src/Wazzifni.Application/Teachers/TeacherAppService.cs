using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Runtime.Caching;
using Abp.UI;
using AutoMapper;
using ITLand.StemCells.Teachers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wazzifni;
using Wazzifni.Domain.Attachments;
using Wazzifni.Domain.Cities;
using Wazzifni.Domain.Teachers;
using Wazzifni.Localization.SourceFiles;
using Wazzifni.Teachers.Dto;
using Wazzifni.Trainees.Dto;
using static Wazzifni.Enums.Enum;


namespace ITLand.StemCells.Teachers
{
    [ApiExplorerSettings(GroupName = "ch10")]
    public class TeacherAppService : ApplicationService, ITeacherAppService
    {
        private readonly IRepository<Teacher> _repository;
        private readonly IMapper _mapper;
        private readonly IAttachmentManager _attachmentManager;

        public TeacherAppService(IRepository<Teacher> repository, IMapper mapper , IAttachmentManager attachmentManager)
        {
            _repository = repository;
            _mapper = mapper;
            _attachmentManager = attachmentManager;
        }


        public async Task<bool> Create(CreateTeacherDto input)
        {
            var Teacher = _mapper.Map<Teacher>(input);

          
            await _repository.InsertAndGetIdAsync(Teacher);
            await UnitOfWorkManager.Current.SaveChangesAsync();

            if (input.AttachmentId != 0)
            {
                await _attachmentManager.CheckAndUpdateRefIdAsync(input.AttachmentId, AttachmentRefType.Teacher, Teacher.Id);
            }

            return true;
        }

        public async Task<PagedResultDto<LiteTeacherDto>> GetAll(PagedTeachersResultRequestDto input)
        {
            var query = _repository.GetAll();

            query = ApplyFiltering(query, input);
            query = ApplySorting(query, input);

            var totalCount = await query.CountAsync();

            var pagedQuery = ApplyPaging(query, input);

            var items = await pagedQuery
                .Select(x => _mapper.Map<LiteTeacherDto>(x))
                .ToListAsync();

           var result = new PagedResultDto<LiteTeacherDto>(totalCount, items);

            var attachments = await _attachmentManager.GetListByRefAsync(result.Items.Select(x => (long)x.Id).ToList(), AttachmentRefType.Teacher);

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

        public async Task<bool> Update(UpdateTeacherDto input)
        {

            var teacher = await _repository.GetAll().Where(x=>x.Id == input.Id).FirstOrDefaultAsync();

            if (teacher is null)
            {
                throw new UserFriendlyException(string.Format(Exceptions.ObjectWasNotFound, "Tokens.Teacher"));
            }

            _mapper.Map(input, teacher);

            await _repository.UpdateAsync(teacher);
            await UnitOfWorkManager.Current.SaveChangesAsync();

            var oldAttachment = await _attachmentManager.GetElementByRefAsync(teacher.Id, AttachmentRefType.Teacher);

            if (input.AttachmentId == 0 && oldAttachment != null)
            {
                await _attachmentManager.DeleteRefIdAsync(oldAttachment);
            }
            else if (input.AttachmentId != 0 && oldAttachment is not null)
            {
                if (oldAttachment.Id != input.AttachmentId)
                {
                    await _attachmentManager.DeleteRefIdAsync(oldAttachment);
                    await _attachmentManager.CheckAndUpdateRefIdAsync(
                     input.AttachmentId, AttachmentRefType.Teacher, teacher.Id);
                }
            }
            else if (input.AttachmentId != 0)
            {
                await _attachmentManager.CheckAndUpdateRefIdAsync(input.AttachmentId, AttachmentRefType.Teacher, teacher.Id);
            }

            return true;
        }



        public async Task<TeacherDetailsDto> GetAsync(EntityDto<long> input)
        {
            var teacher = await _repository.GetAll().Where(x => x.Id == input.Id).FirstOrDefaultAsync();

            var result = _mapper.Map<TeacherDetailsDto>(teacher);
            var logo = await _attachmentManager.GetElementByRefAsync(result.Id, AttachmentRefType.Teacher);
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

        private IQueryable<Teacher> ApplyFiltering(IQueryable<Teacher> query, PagedTeachersResultRequestDto input)
        {

            if (!string.IsNullOrEmpty(input.Keyword))
            {
                query = query.Where(x => x.Name.Contains(input.Keyword) || x.About.Contains(input.Keyword));
            }

            return query;
        }

        private IQueryable<Teacher> ApplySorting(IQueryable<Teacher> query, PagedTeachersResultRequestDto input)
        {

            query = query.OrderByDescending(x => x.CreationTime);
            return query;
        }

        private IQueryable<Teacher> ApplyPaging(IQueryable<Teacher> query, PagedTeachersResultRequestDto input)
        {
            return query.Skip(input.SkipCount).Take(input.MaxResultCount);
        }
    }
}
