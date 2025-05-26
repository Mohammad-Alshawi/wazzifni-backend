using System;
using Abp.Application.Services.Dto;
using Wazzifni.Companies.Dto;
using Wazzifni.Courses.Dto;
using Wazzifni.Trainees.Dto;

namespace Wazzifni.CourseComments.Dto
{
    public class CourseCommentLiteDto : EntityDto<long>
    {
        public TraineeLiteDto Trainee { get; set; }

        public SuperLiteUserDto User { get; set; }

        public long UserId { get; set; }

        public CourseLiteDto Course { get; set; }
        public string Content { get; set; }
        public DateTime CreationTime { get; set; }

        public bool IsForMe { get; set; }
    }
}
