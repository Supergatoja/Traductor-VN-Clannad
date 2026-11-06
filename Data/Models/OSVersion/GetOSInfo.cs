
namespace SeenExtractor.Data.Models.OSVersion
{
    public static class GetOSInfo
    {
        public static int OSBuild => Environment.OSVersion.Version.Build;
        public static int OSMajor => Environment.OSVersion.Version.Major;
    }
}
