using Abp.Application.Services.Dto;
using Wazzifni.Users.Dto;

namespace Wazzifni.Teachers.Dto
{
    public class LiteTeacherDto : EntityDto
    {
        public string Name { get; set; }
        public string About { get; set; }
        public LiteAttachmentDto Image { get; set; }

        //public UserDto User { get; set; }
    }
}
