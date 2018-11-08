namespace SIS.HTTP.Common
{
    public static class GlobalConstants
    {
	public const string CookieRequestHeaderKey = "Cookie";
	public const string CookieResponseHeaderKey = "Set-Cookie";
	public const string ContentTypeHeaderKey = "Content-Type";
	public const string HostHeaderKey = "Host";
	public const string HtmlContentHeaderValue = "text/html";
	public const int HttpCookieDefaultLifetimeInDays = 3;
	public const string HttpOneProtocolFragment = "HTTP/1.1";
	public const string LocationHeaderKey = "Location";
	public const int SecondsInDay = 86400;
	public const string SessionCookieKey = "SIS_ID";
	public const string TextContentHeaderValue = "text/plain";
	public const string UrlPattern = @"^(?<path>\/[^?#\r\n]*)(?<query>\?[^?#\r\n]*)?(?<fragment>\#[^?#\r\n]+$)?";
    }
}
