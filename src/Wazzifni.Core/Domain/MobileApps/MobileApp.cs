using System;
using Abp.Domain.Entities;

namespace Wazzifni.Domain.MobileApps;

public class MobileApp : Entity
{
    public AppTypes AppType { get; set; }
    public SystemType SystemType { get; set; }
    public int VersionCode { get; set; }
    public string VersionNumber { get; set; }
    public string Description { get; set; }
    public UpdateOptions UpdateOptions { get; set; }

    public bool IsPublished { get; set; } = true;

    public DateTime CreationTime { get; set; }
}

public enum SystemType : byte
{
    Android = 1,
    Ios = 2,
}
public enum AppTypes : byte
{
    Basic = 1,
    Business = 2,
    Both = 3
}
public enum UpdateOptions : byte
{
    Optional = 1,
    Mandatory = 2,
    Nothing = 3
}