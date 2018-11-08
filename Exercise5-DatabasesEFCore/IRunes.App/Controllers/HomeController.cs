using System;
using IRunes.App.Common;
using IRunes.App.Controllers.Contracts;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;

namespace IRunes.App.Controllers
{
    public class HomeController : BaseController, IHomeController
    {
	public HomeController(IServiceProvider services) : base(services) { }

	public IHttpResponse Index(IHttpRequest request)
	{
	    if (request.Path.Equals("/"))
	    {
		return BuildView(Constants.HomeViewRoute, request);
	    }
	    return BuildView(request.Path, request);
	}
    }
}
