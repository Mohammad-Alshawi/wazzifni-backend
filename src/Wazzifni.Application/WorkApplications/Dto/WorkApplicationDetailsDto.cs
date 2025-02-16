using Abp.Application.Services.Dto;
using System;
using Wazzifni.Profiles.Dto;
using Wazzifni.WorkPosts.Dto;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.WorkApplications.Dto
{
    public class WorkApplicationDetailsDto : EntityDto<long>
    {
        public WorkPostLiteDto WorkPost { get; set; }
        public WorkApplicationStatus Status { get; set; }
        public string Description { get; set; }

        public ProfileLiteDto Profile { get; set; }
        public string RejectReason { get; set; }

        public DateTime CreationTime { get; set; }

    }
}
