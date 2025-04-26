using AutoMapper;
using Wazzifni.Cities.Dto;
using Wazzifni.CourseCategories.Dto;
using Wazzifni.CourseComments.Dto;
using Wazzifni.Domain.Cities;
using Wazzifni.Domain.CourseCategories;
using Wazzifni.Domain.CourseComments;

namespace Wazzifni.CourseComments.Mapper
{
    public class CourseCommentMapProfile : Profile
    {
        public CourseCommentMapProfile()
        {
            CreateMap<CreateCourseCommentDto, CourseComment>();
            CreateMap<UpdateCourseCommentDto, CourseComment>();
            CreateMap<CourseCommentLiteDto, CourseComment>();
            CreateMap<CourseComment, CourseCommentLiteDto>();
            CreateMap<CourseComment, CourseCommentDetailsDto>();
        }
    }
}
