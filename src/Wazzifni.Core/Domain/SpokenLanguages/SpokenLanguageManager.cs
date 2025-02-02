using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wazzifni.Domain.SpokenLanguages;

public class SpokenLanguageManager : ISpokenLanguageManager
{
    private readonly IRepository<SpokenLanguage> repository;

    public SpokenLanguageManager(IRepository<SpokenLanguage> repository)
    {
        this.repository = repository;
    }

    public async Task<SpokenLanguage> Get(int spokenLanguageId)
    {
        return await repository.GetAsync(spokenLanguageId) ??
            throw new EntityNotFoundException(typeof(SpokenLanguage), spokenLanguageId);
    }

    public async Task<List<SpokenLanguage>> Get(List<int> spokenLanguageIds)
    {
        var entities = await repository.GetAll()
            .Where(x => spokenLanguageIds.Contains(x.Id))
            .ToListAsync();

        if (spokenLanguageIds.Count != entities.Count)
        {
            var missingIds = spokenLanguageIds.Except(entities.Select(x => x.Id)).Select(x => x).ToList();
            if (missingIds.Count != 0) throw new EntityNotFoundException(typeof(SpokenLanguage), missingIds);
        }

        return entities;
    }
}
