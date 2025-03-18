using Wazzifni.Advertisiments.Dto;
using Wazzifni.CrudAppServiceBase;


namespace Wazzifni.Advertisiments
{
    public interface IAdvertisimentAppService : IWazzifniAsyncCrudAppService<AdvertisimentDetailsDto, int, LiteAdvertisimentDto, PagedAdvertisimentResultRequestDto,
        CreateAdvertisimentDto, UpdateAdvertisimentDto>
    {

    }
}
