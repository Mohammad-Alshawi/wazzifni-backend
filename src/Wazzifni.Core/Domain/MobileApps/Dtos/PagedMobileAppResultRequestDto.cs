using Abp.Application.Services.Dto;

namespace Wazzifni.Domain.MobileApps.Dtos
{
    public class PagedMobileAppResultRequestDto : PagedResultRequestDto
    {
        public AppTypes? AppType { get; set; }
        public SystemType? SystemType { get; set; }
        public UpdateOptions? UpdateOptions { get; set; }

        public bool? IsPublished { get; set; }

    }
}
