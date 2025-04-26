using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using Wazzifni.CourseCategories.Dto;

namespace Wazzifni.CourseComments.Dto
{
    public class UpdateCourseCommentDto : CreateCourseCommentDto ,IEntityDto<long>
    {
        [Required]
        public long Id { get; set; }

    }
}
