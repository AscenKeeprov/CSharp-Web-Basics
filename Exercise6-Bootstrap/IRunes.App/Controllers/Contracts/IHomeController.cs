using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;

namespace IRunes.App.Controllers.Contracts
{
    public interface IHomeController
    {
	IHttpResponse Index(IHttpRequest request);
    }
}
