namespace SIS.Framework.Common
{
    public static class Constants
    {
	public const string AppPathPattern = @"(?:.+)(?<appPath>[A-Z]\:.+)(?:\/bin\/.+)";
	public const string ResourcePattern = @".*\/(?<fileName>[^\/]+[\.](?<fileType>[a-zA-z0-9]+))";
    }
}
