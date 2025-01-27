using static Wazzifni.Enums.Enum;

namespace Wazzifni.Users.Dto
{
    public class PagedUserCountDto
    {
        public string? Name { get; set; }
        public string? DialCode { get; set; }
        public bool? IsActive { get; set; }
        public UserType? UserType { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
    }
}
