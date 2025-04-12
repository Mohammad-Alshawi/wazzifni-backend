using AutoMapper;
using Wazzifni.CourseTags.Dto;
using Wazzifni.Domain.CourseTags;
using Wazzifni.Universities.Dto;



namespace Wazzifni.CourseTags.Mapper
{
    public class CourseTagMapProfile : Profile
    {
        public CourseTagMapProfile()
        {
            CreateMap<CreateCourseTagDto, CourseTag>();
            CreateMap<CreateCourseTagDto, CourseTagDto>();
            CreateMap<CourseTagDto, CourseTag>();
            CreateMap<UpdateCourseTagDto, CourseTag>();

        }
    }
}
