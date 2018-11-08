using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;

namespace IRunes.App.Controllers.Contracts
{
    public interface IUsersController
    {
	IHttpResponse LoginGet(IHttpRequest request);
	IHttpResponse LoginPost(IHttpRequest request);
	IHttpResponse RegisterGet(IHttpRequest request);
	IHttpResponse RegisterPost(IHttpRequest request);
    }
}
