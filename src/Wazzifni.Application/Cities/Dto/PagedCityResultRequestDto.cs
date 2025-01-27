using Abp.Application.Services.Dto;

namespace Wazzifni.Cities.Dto
{
    public class PagedCityResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
        public int? CountryId { get; set; }
        public bool? ForWanted { get; set; }
        public bool? isActive { get; set; }
    }
}
