using AutoMapper;
using Wazzifni.Awards;
using Wazzifni.Domain.Educations;
using Wazzifni.Domain.SpokenLanguages;
using Wazzifni.Domain.Trainees;
using Wazzifni.Domain.WorkExperiences;
using Wazzifni.Trainees.Dto;




namespace Wazzifni.Trainees.Mapper
{
    public class TraineeMapTrainee : Profile
    {
        public TraineeMapTrainee()
        {

            CreateMap<CreateTraineeDto, Trainee>();
            CreateMap<CreateTraineeDto, TraineeDetailsDto>();
            CreateMap<TraineeDetailsDto, Trainee>();
            CreateMap<Trainee, TraineeDetailsDto>();
            CreateMap<UpdateTraineeDto, Trainee>();
            CreateMap<Trainee, TraineeLiteDto>();

        



        }
    }
}
