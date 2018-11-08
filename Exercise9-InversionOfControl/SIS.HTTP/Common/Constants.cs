namespace SIS.HTTP.Common
{
    public static class Constants
    {
	public const string CookieRequestHeaderKey = "Cookie";
	public const string CookieResponseHeaderKey = "Set-Cookie";
	public const string ContentDispositionHeaderKey = "Content-Disposition";
	public const string ContentLengthHeaderKey = "Content-Length";
	public const string ContentTypeHeaderKey = "Content-Type";
	public const string DefaultSessionUsername = "Guest";
	public const string ExpiresCookieKey = "Expires";
	public const string HostHeaderKey = "Host";
	public const string HtmlContentTypeHeaderValue = "text/html; charset=utf-8";
	public const int HttpCookieDefaultLifetimeInDays = 3;
	public const int HttpCookieDefaultMaxAgeInSeconds = 259200;
	public const string HttpCookieDefaultPath = "/";
	public const string HttpOneProtocolFragment = "HTTP/1.1";
	public const string HttpOnlyCookieKey = "HttpOnly";
	public const string IconContentTypeHeaderValue = "image/x-icon";
	public const string LocationHeaderKey = "Location";
	public const string MaxAgeCookieKey = "Max-Age";
	public const string PathCookieKey = "Path";
	public const int SecondsInDay = 86400;
	public const string SessionAuthenticationKey = "@IsAuthenticated";
	public const string SessionCookieKey = "SIS-SesId";
	public const string SessionUsernameKey = "@Username";
	public const string TextContentHeaderValue = "text/plain; charset=utf-8";
	public const string UrlPattern = @"^(?<path>\/[^?#\r\n]*)(?<query>\?[^?#\r\n]*)?(?<fragment>\#[^?#\r\n]+$)?";
	public const string WWWAuthenticateHeaderKey = "WWW-Authenticate";
    }
}
