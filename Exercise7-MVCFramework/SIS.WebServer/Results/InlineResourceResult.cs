using SIS.HTTP.Common;
using SIS.HTTP.Enumerations;
using SIS.HTTP.Headers;
using SIS.HTTP.Responses;

namespace SIS.WebServer.Results
{
    public class InlineResourceResult : HttpResponse
    {
	public InlineResourceResult(byte[] content) : base(HttpResponseStatusCode.OK)
	{
	    Content = content;
	    Headers.AddHeader(new HttpHeader(Constants.ContentDispositionHeaderKey, "inline"));
	    Headers.AddHeader(new HttpHeader(Constants.ContentLengthHeaderKey, Content.Length.ToString()));
	}
    }
}
