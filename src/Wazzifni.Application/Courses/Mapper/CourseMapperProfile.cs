using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Wazzifni.CourseCategories.Dto;
using Wazzifni.Courses.Dto;
using Wazzifni.Domain.CourseCategories;
using Wazzifni.Domain.Courses;

namespace Wazzifni.Courses.Mapper
{
    public class CourseMapperProfile : Profile
    {
        public CourseMapperProfile()
        {
            CreateMap<CreateCourseDto, Course>();
            CreateMap<UpdateCourseDto, Course>();
            CreateMap<CourseTranslation, CourseTranslationDto>();
            CreateMap<CourseTranslationDto, CourseTranslation>();

        }


    }
}
