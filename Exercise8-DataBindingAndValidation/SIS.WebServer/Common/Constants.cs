namespace SIS.WebServer.Common
{
    public static class Constants
    {
	public const string DefaultResourcesDir = @"\Resources\";
	public const string HttpContentLengthKey = HTTP.Common.Constants.ContentLengthHeaderKey;
	public const string HttpSessionKey = HTTP.Common.Constants.SessionCookieKey;
	public const string LinuxOpenCommand = "xdg-open";
	public const string LocalHostName = "localhost";
	public const string MacOSOpenCommand = "open";
	public const string ResourceNotFoundMessage = "<h1 style='color: orangered'>{0} {1} not found.</h1>\r\n";
	public const string ResourcePattern = @".*\/(?<fileName>[^\/]+[\.](?<fileType>[a-zA-z0-9]+))";
	public const string WindowsCommandPrompt = "cmd.exe";
	public const string WindowsStartCommand = "/C START {0} /B";
    }
}
