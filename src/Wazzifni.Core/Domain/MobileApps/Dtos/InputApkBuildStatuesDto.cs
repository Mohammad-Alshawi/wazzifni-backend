namespace Wazzifni.Domain.MobileApps.Dtos
{
    public class InputApkBuildStatuesDto
    {
        public AppTypes AppType { get; set; }
        public SystemType SystemType { get; set; }
        public int VersionCode { get; set; }

    }
    public class OutputApkBuildStatuesDto
    {
        public UpdateOptions UpdateOptions { get; set; }
        public bool ApkIsNotFound { get; set; }

        public bool WithBroker { get; set; }
        public bool WithProject { get; set; }
        public bool WithServiceProviders { get; set; }

        public string AndroidLink { get; set; }
        public string IosLink { get; set; }
    }
    public class InputApkNuildStatuesDto
    {
        public int Id { get; set; }

        public UpdateOptions UpdateOptions { get; set; }
    }

    public class OutputBooleanStatuesDto
    {
        public bool BooleanStatues { get; set; }
    }
}
