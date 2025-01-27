namespace Wazzifni.Authorization.Accounts.Dto
{
    public class ProfileInfoDto
    {
        public string RegistrationFullName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string EmailAddress { get; set; }
        public string DialCode { get; set; }
        public string PhoneNumber { get; set; }
        public LiteAttachmentDto ProfilePhoto { get; set; }


    }
}
