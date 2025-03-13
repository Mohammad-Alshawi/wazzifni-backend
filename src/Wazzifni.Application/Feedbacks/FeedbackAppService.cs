using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Wazzifni.Domain.Feedbacks;
using Wazzifni.Feedbacks.Dto;

namespace ITLand.StemCells.Feedbacks
{
    [ApiExplorerSettings(GroupName = "ch10")]
    public class FeedbackAppService : ApplicationService, IFeedbackAppService
    {
        private readonly IRepository<Feedback, long> _repository;
        private readonly IMapper _mapper;

        public FeedbackAppService(IRepository<Feedback, long> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<bool> Create(CreateFeedbackDto input)
        {
            var feedback = _mapper.Map<Feedback>(input);
            feedback.UserId = AbpSession.UserId;

            await _repository.InsertAndGetIdAsync(feedback);
            await UnitOfWorkManager.Current.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResultDto<LiteFeedbackDto>> GetAll(PagedFeedbacksResultRequestDto input)
        {
            var query = _repository.GetAll();

            query = ApplyFiltering(query, input);
            query = ApplySorting(query, input);

            var totalCount = await query.CountAsync();

            var pagedQuery = ApplyPaging(query, input);

            var items = await pagedQuery
                .Select(x => _mapper.Map<LiteFeedbackDto>(x))
                .ToListAsync();

            return new PagedResultDto<LiteFeedbackDto>(totalCount, items);
        }



        private IQueryable<Feedback> ApplyFiltering(IQueryable<Feedback> query, PagedFeedbacksResultRequestDto input)
        {
            query = query.Include(x => x.User);

            if (!string.IsNullOrEmpty(input.Keyword))
            {
                query = query.Where(x => x.Description.Contains(input.Keyword));
            }

            return query;
        }

        private IQueryable<Feedback> ApplySorting(IQueryable<Feedback> query, PagedFeedbacksResultRequestDto input)
        {

            query = query.OrderByDescending(x => x.CreationTime);
            return query;
        }

        private IQueryable<Feedback> ApplyPaging(IQueryable<Feedback> query, PagedFeedbacksResultRequestDto input)
        {
            return query.Skip(input.SkipCount).Take(input.MaxResultCount);
        }
    }
}
