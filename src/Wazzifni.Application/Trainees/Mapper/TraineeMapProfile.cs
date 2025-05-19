using AutoMapper;
using Wazzifni.Awards;
using Wazzifni.Companies.Dto;
using Wazzifni.Domain.Educations;
using Wazzifni.Domain.SpokenLanguages;
using Wazzifni.Domain.Trainees;
using Wazzifni.Domain.WorkExperiences;
using Wazzifni.Trainees.Dto;
using Wazzifni.Users.Dto;




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
            CreateMap<Trainee, TraineeLiteDto>().AfterMap((src, dest) =>
            {
                if (dest.User == null)
                    dest.User = new SuperLiteUserDto(); 

                dest.User.EmailAddress = src.EmailAddress;
            }); ;

        



        }
    }
}
