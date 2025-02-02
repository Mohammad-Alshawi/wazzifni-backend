using Wazzifni.CrudAppServiceBase;
using Wazzifni.SpokenLanguages.DTOs;

namespace Tyoutor.SpokenLanguages.Services;

public interface ISpokenLanguagesAppService : IWazzifniAsyncCrudAppService<SpokenLanguageDetailsDto, int, LiteSpokenLanguageDto, PagedSpokenLanguageRequestResultDto,
        CreateSpokenLanguageDto, UpdateSpokenLanguageDto>
{

}
