using System;
using Abp.Application.Services.Dto;

namespace Wazzifni.Domain.MobileApps.Dtos;

public class LiteMobileAppDto : EntityDto
{
    public AppTypes AppType { get; set; }
    public SystemType SystemType { get; set; }
    public int VersionCode { get; set; }
    public string VersionNumber { get; set; }
    public string Description { get; set; }
    public UpdateOptions UpdateOptions { get; set; }


    //to remove

    public bool WithNewHome { get; set; } = true;
    public bool WithBroker { get; set; } = true;
    public bool WithProject { get; set; } = true;
    public bool WithServiceProviders { get; set; } = true;

    public bool IsPublished { get; set; }
    public DateTime CreationTime { get; set; }
}
