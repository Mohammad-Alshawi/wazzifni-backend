using Abp.Application.Services.Dto;

namespace Wazzifni.Trainees.Dto
{
    public class UpdateTraineeDto : CreateTraineeDto, IEntityDto<long>
    {
        public long Id { get; set; }

        public long TraineePhotoId { get; set; }

        public string RegistrationFullName { get; set; }
        public string EmailAddress { get; set; }


    }
}
