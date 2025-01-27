using Abp.Application.Services.Dto;

namespace Wazzifni.Domain.Companies.Dto
{
    public class CompanyContactDetailsDto : EntityDto
    {
        public string DialCode { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string WebSite { get; set; }
    }
}
