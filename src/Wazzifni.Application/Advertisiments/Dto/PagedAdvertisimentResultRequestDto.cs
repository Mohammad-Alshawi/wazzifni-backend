using Abp.Application.Services.Dto;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Advertisiments.Dto
{
    public class PagedAdvertisimentResultRequestDto : PagedResultRequestDto
    {

        public string Keyword { get; set; }

        public int? WorkPostId { get; set; }

        public UserType? Type { get; set; }

        //public PositionForAdvertisiment? Position { get; set; }

    }
}
