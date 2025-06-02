namespace Wazzifni.Otp.Dto
{
    public class OtpRequestDto
    {
        public string Recipient { get; set; }
        public string SenderId { get; set; }
        public string Type { get; set; } = "whatsapp";
        public string Message { get; set; }
        public string Lang { get; set; } = "en";
    }

}
