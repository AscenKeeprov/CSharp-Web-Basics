using System.Linq;
using System.Runtime.CompilerServices;
using Panda.Models.Enumerations;
using SIS.Framework.ActionResults;
using SIS.Framework.Controllers;

namespace Panda.App.Controllers
{
    public class BaseController : Controller
    {
	protected override IViewable View([CallerMemberName] string actionName = "")
	{
	    Model["AdminOptionDisplay"] = "none";
	    Model["GuestOptionDisplay"] = "flex";
	    Model["LoggedInOptionDisplay"] = "none";
	    Model["UserOptionDisplay"] = "none";
	    if (Identity != null)
	    {
		Model["GuestOptionDisplay"] = "none";
		Model["LoggedInOptionDisplay"] = "flex";
		if (Identity.Roles.Contains(Role.Admin.ToString()))
		{
		    Model["AdminOptionDisplay"] = "flex";
		}
		if (Identity.Roles.Contains(Role.User.ToString()))
		{
		    Model["UserOptionDisplay"] = "flex";
		}
	    }
	    return base.View(actionName);
	}
    }
}
