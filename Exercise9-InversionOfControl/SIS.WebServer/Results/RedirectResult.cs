using SIS.HTTP.Common;
using SIS.HTTP.Enumerations;
using SIS.HTTP.Headers;
using SIS.HTTP.Responses;

namespace SIS.WebServer.Results
{
    public class RedirectResult : HttpResponse
    {
	public RedirectResult(string location) : base(HttpResponseStatusCode.SeeOther)
	{
	    Headers.AddHeader(new HttpHeader(Constants.LocationHeaderKey, location));
	}
    }
}
