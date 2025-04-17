using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace Wazzifni.Courses.Dto
{
    public class UpdateCourseDto : CreateCourseDto, IEntityDto
    {
        public int Id { get; set; }
    }
}
