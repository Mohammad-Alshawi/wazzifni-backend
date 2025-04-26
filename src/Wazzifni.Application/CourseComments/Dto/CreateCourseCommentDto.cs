using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wazzifni.CourseComments.Dto
{
    public class CreateCourseCommentDto
    {
        public int CourseId { get; set; }
        public string Content { get; set; }
    }
}
