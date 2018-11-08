using System.Text;
using SIS.HTTP.Common;
using SIS.HTTP.Enums;
using SIS.HTTP.Headers;
using SIS.HTTP.Responses;

namespace SIS.WebServer.Results
{
    public class HtmlResult : HttpResponse
    {
	public HtmlResult(string content, HttpResponseStatusCode responseStatusCode)
	    : base(responseStatusCode)
	{
	    Headers.Add(new HttpHeader(Constants.ContentTypeHeaderKey, Constants.HtmlContentTypeHeaderValue));
	    Headers.Add(new HttpHeader(Constants.ContentLengthHeaderKey, content.Length.ToString()));
	    Content = Encoding.UTF8.GetBytes(content);
	}
    }
}
