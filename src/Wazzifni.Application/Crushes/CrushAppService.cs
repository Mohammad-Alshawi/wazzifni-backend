using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wazzifni.Crushes.Dto;
using Wazzifni.Domain.Crushes;
using Wazzifni.Domain.Crushes.Dto;

namespace Wazzifni.Crushes
{
    public class CrushAppService : ApplicationService, ICrushAppService
    {
        private readonly IRepository<Crush> _crushRepository;
        private readonly IMapper _mapper;

        public CrushAppService(IRepository<Crush> crushRepository, IMapper mapper)
        {
            _crushRepository = crushRepository;
            _mapper = mapper;
        }



        [HttpPost, AbpAllowAnonymous]
        public async Task<IActionResult> CreateCrush(CreateCrushDto input)
        {
            try
            {
                var crush = new Crush();
                crush.Name = input.Name;
                crush.Description = input.Description;
                await _crushRepository.InsertAndGetIdAsync(crush);

                return new OkObjectResult(crush);
            }
            catch (Exception ex)
            {

                return new OkObjectResult("Operation failed");
            }
        }



        public async Task<PagedResultDto<LiteCrushDto>> GetAll(PagedCrushsResultRequestDto input)
        {
            var query = _crushRepository.GetAll();

            query = ApplyFiltering(query, input);
            query = ApplySorting(query, input);

            var totalCount = await query.CountAsync();

            var pagedQuery = ApplyPaging(query, input);

            var items = await pagedQuery
                .Select(x => _mapper.Map<LiteCrushDto>(x))
                .ToListAsync();

            return new PagedResultDto<LiteCrushDto>(totalCount, items);
        }



        private IQueryable<Crush> ApplyFiltering(IQueryable<Crush> query, PagedCrushsResultRequestDto input)
        {

            if (!string.IsNullOrEmpty(input.Keyword))
            {
                query = query.Where(x => x.Description.Contains(input.Keyword));
            }

            return query;
        }

        private IQueryable<Crush> ApplySorting(IQueryable<Crush> query, PagedCrushsResultRequestDto input)
        {

            query = query.OrderByDescending(x => x.CreationTime);
            return query;
        }

        private IQueryable<Crush> ApplyPaging(IQueryable<Crush> query, PagedCrushsResultRequestDto input)
        {
            return query.Skip(input.SkipCount).Take(input.MaxResultCount);
        }
    }
}
