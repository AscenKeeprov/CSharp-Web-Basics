using SIS.HTTP.Cookies.Contracts;
using SIS.HTTP.Enums;
using SIS.HTTP.Headers.Contracts;

namespace SIS.HTTP.Responses.Contracts
{
    public interface IHttpResponse
    {
	HttpResponseStatusCode StatusCode { get; set; }
	IHttpCookieCollection Cookies { get; }
	IHttpHeaderCollection Headers { get; }
	byte[] Content { get; set; }
	byte[] GetBytes();
    }
}
