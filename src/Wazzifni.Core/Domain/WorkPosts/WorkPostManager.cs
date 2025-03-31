using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wazzifni.Domain.WorkPosts
{
    public class WorkPostManager : DomainService, IWorkPostManager
    {
        private readonly IRepository<WorkPost, long> _repository;

        public WorkPostManager(IRepository<WorkPost, long> repository)
        {
            _repository = repository;
        }


        public async Task<WorkPost> GetEntityByIdAsync(long workPostId)
        {
            return await _repository
                .GetAll().Include(x => x.Company).ThenInclude(x => x.Translations)
                .Include(x => x.Company).ThenInclude(x => x.City).ThenInclude(x => x.Translations)
                .Include(x => x.Company).ThenInclude(x => x.User)
                .AsNoTracking().Where(x => x.Id == workPostId).FirstOrDefaultAsync();
        }
        public async Task<WorkPost> GetEntityByIdAsTrackingAsync(long workPostId)
        {
            return await _repository
                .GetAll().Include(x => x.Company).ThenInclude(x => x.Translations)
                .Include(x => x.Company).ThenInclude(x => x.User)
                .Include(x => x.Applications)
                .Where(x => x.Id == workPostId).FirstOrDefaultAsync();
        }

        public async Task<WorkPost> GetEntityWithoutUserByIdAsync(long workPostId)
        {
            return await _repository
                .GetAll().Include(x => x.Company).ThenInclude(x => x.Translations)
                .Include(x => x.Company).ThenInclude(x => x.City).ThenInclude(x => x.Translations)
                .AsNoTracking().Where(x => x.Id == workPostId).FirstOrDefaultAsync();
        }


        public async Task<List<WorkPost>> GetAllWorkPostInAvaibiltyStatuesAndClosed()
        {
            var daysForMarkWorkPostAsUnavailable = 3;
            var workPosts = await _repository.GetAll().Where(x => x.WorkAvailbility == Enums.Enum.WorkAvailbility.Available
                                && x.IsClosed == true
                                && x.ClosedDate.Value.Date.AddDays(daysForMarkWorkPostAsUnavailable) <= DateTime.Now.Date)
                               .ToListAsync();


            foreach (var post in workPosts)
            {
                post.WorkAvailbility = Enums.Enum.WorkAvailbility.Unavilable;
                post.LastModificationTime = DateTime.Now;
            }
            return workPosts;
        }

    }
}
