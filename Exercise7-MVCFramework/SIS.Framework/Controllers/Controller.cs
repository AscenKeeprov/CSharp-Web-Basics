using System.Runtime.CompilerServices;
using SIS.Framework.ActionResults;
using SIS.Framework.ActionResults.Contracts;
using SIS.Framework.Utilities;
using SIS.Framework.Views;
using SIS.HTTP.Requests.Contracts;

namespace SIS.Framework.Controllers
{
    public abstract class Controller
    {
	public IHttpRequest Request { get; set; }

	protected IViewable View([CallerMemberName] string caller = "")
	{
	    //TODO: REVIEW AND REFINE ?!
	    var controllerName = ControllerUtilities.GetControllerName(this);
	    var fullyQualifiedViewName = ControllerUtilities
		.GetFullyQualifiedViewName(controllerName, caller);
	    var view = new View(fullyQualifiedViewName);
	    return new ViewResult(view);
	}

	protected IRedirectable RedirectToAction(string redirectUrl)
	    => new RedirectResult(redirectUrl);
    }
}
