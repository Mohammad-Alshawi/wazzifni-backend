using System;

namespace Wazzifni.Domain.MobileApps.Dtos;

public class CreateMobileAppDto
{
    public AppTypes AppType { get; set; }
    public SystemType SystemType { get; set; }
    public int VersionCode { get; set; }
    public string VersionNumber { get; set; }
    public string Description { get; set; }


    //to remove
    public bool WithNewHome { get; set; }
    public bool WithBroker { get; set; }
    public bool WithProject { get; set; }
    public bool WithServiceProviders { get; set; }


    public bool IsPublished { get; set; } = true;
    public DateTime CreationTime { get; set; }

}
