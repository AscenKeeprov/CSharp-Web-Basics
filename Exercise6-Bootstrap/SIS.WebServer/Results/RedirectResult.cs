using SIS.HTTP.Common;
using SIS.HTTP.Enumerations;
using SIS.HTTP.Headers;

namespace SIS.WebServer.Results
{
    public class RedirectResult : HtmlResult
    {
	public RedirectResult(string location, string content)
	    : base(content, HttpResponseStatusCode.SeeOther)
	{
	    Headers.AddHeader(new HttpHeader(Constants.LocationHeaderKey, location));
	}
    }
}
