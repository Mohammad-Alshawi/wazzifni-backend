using Wazzifni.CrudAppServiceBase;
using Wazzifni.Profiles.Dto;

namespace Wazzifni.Profiles
{
    public interface IProfileAppService : IWazzifniAsyncCrudAppService<ProfileDetailsDto, long, ProfileLiteDto, PagedProfileResultRequestDto,
        CreateProfileDto, UpdateProfileDto>
    {
    }
}
