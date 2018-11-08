using System.Text;
using SIS.HTTP.Common;
using SIS.HTTP.Enumerations;
using SIS.HTTP.Headers;
using SIS.HTTP.Responses;

namespace SIS.WebServer.Results
{
    public class HtmlResult : HttpResponse
    {
	public HtmlResult(string content, HttpResponseStatusCode responseStatusCode)
	    : base(responseStatusCode)
	{
	    Content = Encoding.UTF8.GetBytes(content);
	    Headers.AddHeader(new HttpHeader(Constants.ContentTypeHeaderKey, Constants.HtmlContentTypeHeaderValue));
	    Headers.AddHeader(new HttpHeader(Constants.ContentLengthHeaderKey, Content.Length.ToString()));
	}
    }
}
