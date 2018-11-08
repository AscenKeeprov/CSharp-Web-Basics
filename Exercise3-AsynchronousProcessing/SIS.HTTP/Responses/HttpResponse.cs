using System;
using System.Linq;
using System.Text;
using SIS.HTTP.Common;
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
	    Headers = new HttpHeaderCollection();
	    Content = new byte[0];
	    StatusCode = statusCode;
	}

	public HttpResponseStatusCode StatusCode { get; set; }
	public IHttpHeaderCollection Headers { get; private set; }
	public byte[] Content { get; set; }

	public void AddHeader(IHttpHeader header)
	{
	    Headers.Add(header);
	}

	public byte[] GetBytes()
	{
	    return Encoding.UTF8.GetBytes(ToString()).Concat(Content).ToArray();
	}

	public override string ToString()
	{
	    StringBuilder responseText = new StringBuilder();
	    responseText.Append($"{GlobalConstants.HttpOneProtocolFragment}");
	    responseText.AppendLine($" {(int)StatusCode} {StatusCode}");
	    if (Headers != null && Headers.Count() > 0)
		responseText.AppendLine(Headers.ToString());
	    if (Content.Length > 0) responseText.AppendLine(Environment.NewLine);
	    return responseText.ToString();
	}
    }
}
