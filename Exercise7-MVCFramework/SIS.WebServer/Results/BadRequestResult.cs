using SIS.HTTP.Enumerations;
using SIS.HTTP.Responses;

namespace SIS.WebServer.Results
{
    public class BadRequestResult : HttpResponse
    {
	public BadRequestResult() : base(HttpResponseStatusCode.BadRequest) { }
    }
}
