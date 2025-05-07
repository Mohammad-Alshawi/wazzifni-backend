using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Runtime.Caching;
using Abp.UI;
using KeyFinder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wazzifni.Authorization.Users;
using Wazzifni.Cities.Dto;
using Wazzifni.CourseCategories.Dto;
using Wazzifni.Messages.Dto;
using Wazzifni.CrudAppServiceBase;
using Wazzifni.Domain.Attachments;
using Wazzifni.Domain.Cities;
using Wazzifni.Domain.Countries;
using Wazzifni.Domain.CourseCategories;
using Wazzifni.Domain.Messages;
using Wazzifni.Domain.Courses;
using Wazzifni.Domain.Trainees;
using Wazzifni.Localization.SourceFiles;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Messages
{

    public class MessageAppService :
        WazzifniAsyncCrudAppService<Message, MessageDetailsDto, Guid, MessageLiteDto, PagedMessageResultRequestDto, CreateMessageDto, UpdateMessageDto>,
        IMessageAppService
    {


        private readonly UserManager _userManager;
        private readonly ICacheManager _cacheManager;
        private readonly CountryManager _countryManager;
        private readonly IRepository<Message,Guid> _MessageRepository;
        private readonly ICourseManager _courseManager;
        private readonly ITraineeManager _traineeManager;
        private readonly IAttachmentManager _attachmentManager;


        public MessageAppService(
            UserManager userManager,
            ICacheManager cacheManager,
            CountryManager countryManager,
            IRepository<Message, Guid> MessageRepository,
            ICourseManager courseManager,
            ITraineeManager traineeManager,
            IAttachmentManager attachmentManager
        ) : base(MessageRepository)
        {
            _userManager = userManager;
            _cacheManager = cacheManager;
            _countryManager = countryManager;
            _MessageRepository = MessageRepository;
            _courseManager = courseManager;
            _traineeManager = traineeManager;
            _attachmentManager = attachmentManager;
        }


        [HttpGet, AbpAllowAnonymous]
        public override async Task<MessageDetailsDto> GetAsync(EntityDto<Guid> input)
        {
            return await GetFromDatabase(input);
        }

        [HttpGet, AbpAuthorize]
        public override async Task<PagedResultDto<MessageLiteDto>> GetAllAsync(PagedMessageResultRequestDto input)
        {
            return await GetAllFromDatabase(input);

        }

        [AbpAuthorize]
        public override async Task<MessageDetailsDto> CreateAsync(CreateMessageDto input)
        {

            var Message = ObjectMapper.Map<Message>(input);

            Message.CreationTime = DateTime.UtcNow;
            Message.UserSenderId = AbpSession.UserId.Value;

            if (await _userManager.IsAdminSession())
            {
                if (!input.UserReceiverId.HasValue)
                {
                    throw new UserFriendlyException("Reciver Id Required");
                }
                Message.OwnerIsAdmin = true;
            }

            await Repository.InsertAsync(Message);
            await CurrentUnitOfWork.SaveChangesAsync();

            return MapToEntityDto(Message);
        }


        [AbpAuthorize]       
        public override async Task<MessageDetailsDto> UpdateAsync(UpdateMessageDto input)
        {
            var Message = await _MessageRepository.GetAll().Where(x=>x.Id == input.Id).FirstOrDefaultAsync();

            if (Message is null)
            {
                throw new UserFriendlyException(string.Format(Exceptions.ObjectWasNotFound, "Tokens.Message"));
            }
            if (AbpSession.UserId.Value != Message?.UserSenderId) 
            {
                throw new UserFriendlyException(string.Format(Exceptions.YouCannotDoThisAction));
            }

            MapToEntity(input, Message);

            Message.UserSenderId = AbpSession.UserId.Value;

            if (await _userManager.IsAdminSession())
            {
                if (!input.UserReceiverId.HasValue)
                {
                    throw new UserFriendlyException("Reciver Id Required");
                }
                Message.OwnerIsAdmin = true;
            }
            await _MessageRepository.UpdateAsync(Message);
            await UnitOfWorkManager.Current.SaveChangesAsync();

            return MapToEntityDto(Message);

        }

        [AbpAuthorize]
        public override async Task DeleteAsync(EntityDto<Guid> input)
        {
            var Message = await _MessageRepository.GetAll().Where(x => x.Id == input.Id).FirstOrDefaultAsync();

            if (Message is null)
            {
                throw new UserFriendlyException(string.Format(Exceptions.ObjectWasNotFound, "Tokens.Message"));
            }
            var currentTraineeId = await _traineeManager.GetTraineeIdByUserId(AbpSession.UserId.Value);
            if (!await _userManager.IsAdminSession() && AbpSession.UserId.Value != Message?.UserSenderId)
            {
                throw new UserFriendlyException(string.Format(Exceptions.YouCannotDoThisAction));
            }

            
            await _MessageRepository.DeleteAsync(input.Id);
        }






        private async Task<MessageDetailsDto> GetFromDatabase(EntityDto<Guid> input)
        {
            var Message = await _MessageRepository.GetAll().Include(x=>x.UserSender).Include(x=>x.UserReceiver).Where(x => x.Id == input.Id).FirstOrDefaultAsync();

            var MessageDetailsDto = MapToEntityDto(Message);
     
            return MessageDetailsDto;
        }

        private async Task<PagedResultDto<MessageLiteDto>> GetAllFromDatabase(PagedMessageResultRequestDto input)
        {
            var result = await base.GetAllAsync(input);

            var ItemsIds = result.Items.Select(x => x.Id).ToList();



            foreach (var item in result.Items)
            {
                item.ISent = item.UserSenderId == AbpSession.UserId.Value;
                item.IReceive = item.UserReceiverId == AbpSession.UserId.Value;
            }


            return result;
        }




        protected override IQueryable<Message> CreateFilteredQuery(PagedMessageResultRequestDto input)
        {
            var data = base.CreateFilteredQuery(input);

            data = data.Include(x => x.UserSender).Include(x => x.UserReceiver);


            if (!string.IsNullOrEmpty(input.Keyword))
            {
                var keyword = input.Keyword.ToLower();

                data = data.Where(p =>

                    p.Content.Contains(keyword)           
                );
            }
            if (input.UserSenderId.HasValue)
                data = data.Where(x => x.UserSenderId == input.UserSenderId.Value);

            if (input.UserReceiverId.HasValue)
                data = data.Where(x => x.UserReceiverId == input.UserReceiverId.Value);


            if (input.MessagesIReceived.HasValue)
                data = data.Where(x => x.UserReceiverId == AbpSession.UserId.Value);

            if (input.MessagesISent.HasValue)
                data = data.Where(x => x.UserSenderId == AbpSession.UserId.Value);

            if (input.MessagesReceiveAndSent.HasValue)
                data = data.Where(x => x.UserSenderId == AbpSession.UserId.Value || x.UserReceiverId == AbpSession.UserId.Value);

            if (!input.Keyword.IsNullOrEmpty())
                data = data.Where(x => x.Content.Contains(input.Keyword));

            
            return data;
        }

        protected override IQueryable<Message> ApplySorting(IQueryable<Message> query, PagedMessageResultRequestDto input)
        {
            
            return query.OrderBy(r => r.CreationTime);
        }
    }
}
