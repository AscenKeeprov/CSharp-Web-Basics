using System.Text;
using SIS.HTTP.Enumerations;
using SIS.HTTP.Headers;
using SIS.HTTP.Responses;
using SIS.WebServer.Common;

namespace SIS.WebServer.Results
{
    public class NotFoundResult : HttpResponse
    {
	public NotFoundResult(string resourceType, string resourceName)
	    : base(HttpResponseStatusCode.NotFound)
	{
	    Content = Encoding.UTF8.GetBytes(string.Format(Constants.ResourceNotFoundMessage, resourceType, resourceName));
	    Headers.AddHeader(new HttpHeader(Constants.HttpContentLengthKey, Content.Length.ToString()));
	}
    }
}
