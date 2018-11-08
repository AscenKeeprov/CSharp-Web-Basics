using SIS.HTTP.Common;
using SIS.HTTP.Enums;
using SIS.HTTP.Headers;

namespace SIS.WebServer.Results
{
    public class RedirectResult : HtmlResult
    {
	public RedirectResult(string location, string content)
	    : base(content, HttpResponseStatusCode.SeeOther)
	{
	    Headers.Add(new HttpHeader(Constants.LocationHeaderKey, location));
	}
    }
}
