using Wazzifni.Awards;
using Wazzifni.Domain.Educations;
using Wazzifni.Domain.SpokenLanguages;
using Wazzifni.Domain.WorkExperiences;
using Wazzifni.Profiles.Dto;
using Profile = AutoMapper.Profile;



namespace Wazzifni.Profiles.Mapper
{
    public class ProfileMapProfile : Profile
    {
        public ProfileMapProfile()
        {

            CreateMap<CreateProfileDto, Domain.IndividualUserProfiles.Profile>();
            CreateMap<CreateProfileDto, ProfileDetailsDto>();
            CreateMap<ProfileDetailsDto, Domain.IndividualUserProfiles.Profile>();
            CreateMap<Domain.IndividualUserProfiles.Profile, ProfileDetailsDto>();
            CreateMap<UpdateProfileDto, Domain.IndividualUserProfiles.Profile>();
            CreateMap<Domain.IndividualUserProfiles.Profile, ProfileLiteDto>();

            CreateMap<WorkExperience, WorkExperienceDto>();
            CreateMap<WorkExperienceDto, WorkExperience>();

            CreateMap<EducationDto, Education>();
            CreateMap<Education, EducationDto>();

            CreateMap<AwardDto, Award>();
            CreateMap<Award, AwardDto>();

            CreateMap<SpokenLanguageInputDto, SpokenLanguageValue>();
            CreateMap<SpokenLanguageValue, SpokenLanguageOutputDto>();



        }
    }
}
