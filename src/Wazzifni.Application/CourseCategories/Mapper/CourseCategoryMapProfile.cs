using AutoMapper;
using Wazzifni.Cities.Dto;
using Wazzifni.CourseCategories.Dto;
using Wazzifni.Domain.Cities;
using Wazzifni.Domain.CourseCategories;

namespace Wazzifni.CourseCategories.Mapper
{
    public class CourseCategoryMapProfile : Profile
    {
        public CourseCategoryMapProfile()
        {
            CreateMap<CreateCourseCategoryDto, CourseCategory>();
            CreateMap<CreateCourseCategoryDto, CourseCategoryDto>();
            CreateMap<CourseCategoryDto, CourseCategory>();
            CreateMap<UpdateCourseCategoryDto, CourseCategory>();
            CreateMap<LiteCourseCategory, CourseCategory>();

        }
    }
}
