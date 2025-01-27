using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Users.Dto
{
    //custom PagedResultRequestDto
    public class PagedUserResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
        public bool? IsActive { get; set; }
        public List<string> RoleNames { get; set; }
        [JsonIgnore]
        internal List<long> UserIds { get; set; }
        public UserType? UserType { get; set; }
        public DateTime? CreateDateFrom { get; set; }
        public DateTime? CreateDateTo { get; set; }

        public long? UserId { get; set; }

    }
}
