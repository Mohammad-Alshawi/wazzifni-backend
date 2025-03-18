using static Wazzifni.Enums.Enum;

namespace Wazzifni.Advertisiments.Dto
{
    public class CreateAdvertisimentDto
    {
        public long AttachmentId { get; set; }
        public string? Link { get; set; }
        public long? WorkPostId { get; set; }

        public UserType Type { get; set; }

        //public List<CreateAdvertisimentPositionDto> AdvertisimentPositions { get; set; }
    }
}
