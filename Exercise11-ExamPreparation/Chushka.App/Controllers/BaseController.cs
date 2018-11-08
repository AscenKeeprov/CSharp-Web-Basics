using System.Linq;
using System.Runtime.CompilerServices;
using Chushka.Models.Enumerations;
using SIS.Framework.ActionResults;
using SIS.Framework.Controllers;

namespace Chushka.App.Controllers
{
    public abstract class BaseController : Controller
    {
	protected override IViewable View([CallerMemberName] string actionName = "")
	{
	    Model["AdminOptionDisplay"] = "none";
	    Model["LoggedInOptionDisplay"] = "none";
	    Model["LoggedOutOptionDisplay"] = "flex";
	    if (Identity != null)
	    {
		Model["LoggedInOptionDisplay"] = "flex";
		Model["LoggedOutOptionDisplay"] = "none";
		if (Identity.Roles.Contains(UserRole.Admin.ToString()))
		{
		    Model["AdminOptionDisplay"] = "flex";
		}
	    }
	    return base.View(actionName);
	}
    }
}
