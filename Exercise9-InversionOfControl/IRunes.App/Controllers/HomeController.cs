using IRunes.App.Common;
using IRunes.App.Controllers.Contracts;
using IRunes.App.ViewModels;
using SIS.Framework.ActionResults.Contracts;
using SIS.Framework.Attributes.Methods;
using SIS.Framework.Controllers;

namespace IRunes.App.Controllers
{
    public class HomeController : Controller, IHomeController
    {
	[HttpGet]
	public IActionResult Index(HomeViewModel model)
	{
	    model.Username = Request.Session
		.Parameters[Constants.SessionUsernameKey].ToString();
	    return View(model);
	}
    }
}
