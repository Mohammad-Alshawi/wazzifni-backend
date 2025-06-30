using System;
using Abp.Application.Services.Dto;

namespace Wazzifni.Domain.MobileApps.Dtos;

public class MobileAppDetailsDto : EntityDto
{
    public AppTypes AppType { get; set; }
    public SystemType SystemType { get; set; }
    public int VersionCode { get; set; }
    public string VersionNumber { get; set; }
    public string Description { get; set; }
    public UpdateOptions UpdateOptions { get; set; }


    public bool IsPublished { get; set; }

    public bool ApkIsNotFound { get; set; }
    public OutputMobileLinksDto MobileLinks { get; set; } = new OutputMobileLinksDto();


    public DateTime CreationTime { get; set; }

}


public class OutputMobileLinksDto
{
    public string IosLinkForBasic { get; set; }
    public string AndroidLinkForBasic { get; set; }

    public string IosLinkForBusiness { get; set; }
    public string AndroidLinkForBusiness { get; set; }
}
