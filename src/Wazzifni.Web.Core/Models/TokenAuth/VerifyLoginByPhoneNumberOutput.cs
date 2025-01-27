using static Wazzifni.Enums.Enum;

namespace Wazzifni.Models.TokenAuth
{
    public class VerifyLoginByPhoneNumberOutput
    {
        public string AccessToken { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public UserType UserType { get; set; }


    }
}
