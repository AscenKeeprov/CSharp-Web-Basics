using System.Text;
using SIS.HTTP.Common;
using SIS.HTTP.Enums;
using SIS.HTTP.Headers;
using SIS.HTTP.Responses;

namespace SIS.WebServer.Results
{
    public class TextResult : HttpResponse
    {
	public TextResult(string content, HttpResponseStatusCode responseStatusCode)
	    : base(responseStatusCode)
	{
	    Headers.Add(new HttpHeader(Constants.ContentTypeHeaderKey, Constants.TextContentHeaderValue));
	    Content = Encoding.UTF8.GetBytes(content);
	}
    }
}
