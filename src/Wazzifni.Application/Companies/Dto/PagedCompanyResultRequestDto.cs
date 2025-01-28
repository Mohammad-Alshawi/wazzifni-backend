using Abp.Application.Services.Dto;
using static Wazzifni.Enums.Enum;

namespace Wazzifni.Domain.Companies.Dto
{
    public class PagedCompanyResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }

        public CompanyStatus? Status { get; set; }



    }
}
