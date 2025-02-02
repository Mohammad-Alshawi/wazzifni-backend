using Abp.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wazzifni.Domain.SpokenLanguages;

public interface ISpokenLanguageManager : IDomainService
{
    Task<SpokenLanguage> Get(int spokenLanguageId);
    Task<List<SpokenLanguage>> Get(List<int> spokenLanguageIds);
}
