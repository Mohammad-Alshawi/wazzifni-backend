﻿using System;
using Abp.Application.Services.Dto;
using Wazzifni.Companies.Dto;
using Wazzifni.Courses.Dto;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.CourseRegistrationRequests.Dto
{
    public class CourseRegistrationRequestLiteDto : EntityDto<long>
    {
        public CourseLiteDto Course { get; set; }
        public CourseRegistrationRequestStatus? Status { get; set; }

        //public TraineeLiteDto Trainee { get; set; }
        public SuperLiteUserDto User { get; set; }

        public string RejectReason { get; set; }
        public DateTime CreationTime { get; set; }

        public bool IsSpecial { get; set; }


    }
}
