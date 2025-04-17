using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Wazzifni.Domain.Companies;
using Wazzifni.Domain.Courses;
using Wazzifni.Domain.CourseTags;

namespace Wazzifni.Courses.Dto
{
    [AutoMap(typeof(CourseTranslation))]
    public class CourseTranslationDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string Language { get; set; }

    }
}
