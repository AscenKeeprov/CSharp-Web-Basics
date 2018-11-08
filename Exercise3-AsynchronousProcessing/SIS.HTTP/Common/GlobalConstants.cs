namespace SIS.HTTP.Common
{
    public static class GlobalConstants
    {
	public const string ContentTypeHeaderKey = "Content-Type";
	public const string HostHeaderKey = "Host";
	public const string HtmlContentHeaderValue = "text/html";
	public const string HttpOneProtocolFragment = "HTTP/1.1";
	public const string LocationHeaderKey = "Location";
	public const string TextContentHeaderValue = "text/plain";
	public const string UrlPattern = @"^(?<path>\/[^?#\r\n]*)(?<query>\?[^?#\r\n]*)?(?<fragment>\#[^?#\r\n]+$)?";
    }
}
