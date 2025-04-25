using AutoMapper;
using Wazzifni.CourseRegistrationRequests.Dto;
using Wazzifni.Domain.CourseRegistrationRequests;

namespace Wazzifni.CourseRegistrationRequests.Mapper
{

    public class CourseRegistrationRequestMapperProfile : Profile
    {
        public CourseRegistrationRequestMapperProfile()
        {

            CreateMap<CreateCourseRegistrationRequestDto, CourseRegistrationRequest>();
            CreateMap<CreateCourseRegistrationRequestDto, CourseRegistrationRequestDetailsDto>();
            CreateMap<CourseRegistrationRequestDetailsDto, CourseRegistrationRequest>();
            CreateMap<CourseRegistrationRequest, CourseRegistrationRequestDetailsDto>();
            CreateMap<UpdateCourseRegistrationRequestDto, CourseRegistrationRequest>();
            CreateMap<CourseRegistrationRequest, CourseRegistrationRequestLiteDto>();




        }
    }
}
