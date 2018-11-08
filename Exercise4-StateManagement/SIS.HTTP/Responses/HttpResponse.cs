using System;
using System.Linq;
using System.Text;
using SIS.HTTP.Common;
using SIS.HTTP.Cookies;
using SIS.HTTP.Cookies.Contracts;
using SIS.HTTP.Enums;
using SIS.HTTP.Headers;
using SIS.HTTP.Headers.Contracts;
using SIS.HTTP.Responses.Contracts;

namespace SIS.HTTP.Responses
{
    public class HttpResponse : IHttpResponse
    {
	public HttpResponse() { }

	public HttpResponse(HttpResponseStatusCode statusCode)
	{
	    Cookies = new HttpCookieCollection();
	    Headers = new HttpHeaderCollection();
	    Content = new byte[0];
	    StatusCode = statusCode;
	}

	public HttpResponseStatusCode StatusCode { get; set; }
	public IHttpCookieCollection Cookies { get; }
	public IHttpHeaderCollection Headers { get; private set; }
	public byte[] Content { get; set; }

	public byte[] GetBytes()
	{
	    return Encoding.UTF8.GetBytes(ToString()).Concat(Content).ToArray();
	}

	public override string ToString()
	{
	    StringBuilder response = new StringBuilder();
	    response.Append(GlobalConstants.HttpOneProtocolFragment);
	    response.Append($" {(int)StatusCode} {StatusCode}{Environment.NewLine}");
	    if (Headers.Any()) response.Append(Headers.ToString() + Environment.NewLine);
	    if (Cookies.Any())
	    {
		response.Append(GlobalConstants.CookieResponseHeaderKey);
		response.Append($": {Cookies.ToString()}{Environment.NewLine}");
	    }
	    if (Content.Length > 0) response.Append(Environment.NewLine);
	    return response.ToString();
	}
    }
}
