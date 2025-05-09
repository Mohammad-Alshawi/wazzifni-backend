using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Wazzifni.Authorization.Users;
using Wazzifni.Chats.Dto;
using Wazzifni.Domain.Messages;
using Wazzifni.Messages.Dto;
using Wazzifni.Users.Dto;

namespace ITLand.Wazzifni.Chats
{
    [ApiExplorerSettings(GroupName = "ch10")]
    public class ChatAppService : ApplicationService, IChatAppService
    {
        private readonly IRepository<Chat, long> _repository;
        private readonly IMapper _mapper;
        private readonly UserManager _userManager;

        public ChatAppService(IRepository<Chat, long> repository, IMapper mapper,UserManager userManager)
        {
            _repository = repository;
            _mapper = mapper;
            _userManager = userManager;
        }


        [AbpAuthorize]
        public async Task<PagedResultDto<LiteChatDto>> GetAll(PagedChatsResultRequestDto input)
        {
            if(await _userManager.IsAdminSession()) input.IsAdmin = true;

            var query = _repository.GetAll();

            query = ApplyFiltering(query, input);
            query = ApplySorting(query, input);

            var totalCount = await query.CountAsync();

            var pagedQuery = ApplyPaging(query, input);

            var items = await pagedQuery
                .Select(x => new LiteChatDto
                {
                    Id = x.Id,
                    User = _mapper.Map<UserDto>(x.User),
                    LastMessage = x.Messages
                        .OrderByDescending(m => m.CreationTime)
                        .Select(m => new MessageLiteDto
                        {
                            Id = m.Id,
                            Content = m.Content,
                            CreationTime = m.CreationTime,
                            ISent = m.UserSenderId == AbpSession.UserId.Value,
                            IReceive = m.UserReceiverId == AbpSession.UserId.Value,
                        })
                        .FirstOrDefault()
                     
                })
                .ToListAsync();


            return new PagedResultDto<LiteChatDto>(totalCount, items);
        }



        private IQueryable<Chat> ApplyFiltering(IQueryable<Chat> query, PagedChatsResultRequestDto input)
        {
            query = query.Include(x => x.User);

            if (!string.IsNullOrEmpty(input.Keyword))
            {
            }
            if (input.UserId.HasValue)
            {
                query=query.Where(x=>x.UserId == input.UserId.Value);
            }

            if (!input.IsAdmin)
            {
                query = query.Where(x => x.UserId ==AbpSession.UserId.Value);
            }

            return query;
        }

        private IQueryable<Chat> ApplySorting(IQueryable<Chat> query, PagedChatsResultRequestDto input)
        {

            query = query.OrderByDescending(x => x.CreationTime);
            return query;
        }

        private IQueryable<Chat> ApplyPaging(IQueryable<Chat> query, PagedChatsResultRequestDto input)
        {
            return query.Skip(input.SkipCount).Take(input.MaxResultCount);
        }
    }
}
