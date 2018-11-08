namespace SIS.Framework.Common
{
    public static class Constants
    {
	public const string AppPathPattern = @"(?:.+)(?<appPath>[A-Z]\:.+)(?:\/bin\/.+)";
	public const string FolderSeparator = "/";
	public const string HtmlBodyPlaceholder = "@Body";
	public const string HtmlErrorMessagePlaceholder = "@ErrorMessage";
	public const string HtmlFileExtension = ".html";
	public const string ResourcePattern = WebServer.Common.Constants.ResourcePattern;
	public const string SessionAuthenticationKey = "@IsAuthenticated";
	public const string SessionUsernameKey = "@Username";
    }
}
