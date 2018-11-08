using SIS.HTTP.Common;
using SIS.HTTP.Enums;
using SIS.HTTP.Headers;
using SIS.HTTP.Responses;

namespace SIS.WebServer.Results
{
    public class FaviconResult : HttpResponse
    {
	public FaviconResult(byte[] faviconBytes, HttpResponseStatusCode responseStatusCode)
	    : base(responseStatusCode)
	{
	    Headers.Add(new HttpHeader(Constants.ContentTypeHeaderKey, Constants.IconContentTypeHeaderValue));
	    Headers.Add(new HttpHeader(Constants.ContentLengthHeaderKey, faviconBytes.Length.ToString()));
	    Content = faviconBytes;
	}
    }
}
