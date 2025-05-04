using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace Wazzifni.Teachers.Dto
{
    public class TeacherDetailsDto : EntityDto
    {
        public string Name { get; set; }
        public string About { get; set; }
        public LiteAttachmentDto Image { get; set; }

        public int AssignedCourseCount { get; set; }
    }
}
