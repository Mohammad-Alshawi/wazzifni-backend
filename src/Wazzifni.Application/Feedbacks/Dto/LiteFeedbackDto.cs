using Wazzifni.Users.Dto;

namespace Wazzifni.Feedbacks.Dto
{
    public class LiteFeedbackDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public UserDto User { get; set; }
    }
}
