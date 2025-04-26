using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Wazzifni.Courses.Dto;
using Wazzifni.Trainees.Dto;

namespace Wazzifni.CourseComments.Dto
{
    public class CourseCommentLiteDto :EntityDto<long>
    {
        public TraineeLiteDto Trainee { get; set; }
        public CourseLiteDto Course { get; set; }
        public string Content { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
