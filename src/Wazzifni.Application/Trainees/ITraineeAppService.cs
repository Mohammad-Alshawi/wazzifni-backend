using Wazzifni.CrudAppServiceBase;
using Wazzifni.Trainees.Dto;

namespace Wazzifni.Trainees
{
    public interface ITraineeAppService : IWazzifniAsyncCrudAppService<TraineeDetailsDto, long, TraineeLiteDto, PagedTraineeResultRequestDto,
        CreateTraineeDto, UpdateTraineeDto>
    {
    }
}
