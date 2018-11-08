using System.Text;
using SIS.HTTP.Common;
using SIS.HTTP.Enumerations;
using SIS.HTTP.Headers;
using SIS.HTTP.Responses;

namespace SIS.WebServer.Results
{
    public class UnauthorizedResult : HttpResponse
    {
	public UnauthorizedResult(string content) : base(HttpResponseStatusCode.Unauthorized)
	{
	    Content = Encoding.UTF8.GetBytes(content);
	    Headers.AddHeader(new HttpHeader(Constants.ContentLengthHeaderKey, Content.Length.ToString()));
	    Headers.AddHeader(new HttpHeader(Constants.WWWAuthenticateHeaderKey, "Basic"));
	}
    }
}
