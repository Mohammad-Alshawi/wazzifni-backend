using Wazzifni.Companies.Dto;
using Wazzifni.Profiles.Dto;
using Wazzifni.Trainees.Dto;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Models.TokenAuth
{
    public class VerifyLoginByPhoneNumberOutput
    {
        public string AccessToken { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public UserType UserType { get; set; }

        public CompanyStatus? CompanyStatus { get; set; }
        public int? CompanyId { get; set; }
        public long? ProfileId { get; set; }
        public long? TraineeId { get; set; }

        public CompanyDetailsDto Company { get; set; }
        public ProfileDetailsDto Profile { get; set; }

        public TraineeDetailsDto Trainee { get; set; }


    }
}
