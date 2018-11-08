using System.Runtime.CompilerServices;
using SIS.Framework.ActionResults;
using SIS.Framework.ActionResults.Contracts;
using SIS.Framework.Common;
using SIS.Framework.Controllers.Contracts;
using SIS.Framework.Models;
using SIS.Framework.Views;
using SIS.HTTP.Requests.Contracts;

namespace SIS.Framework.Controllers
{
    public abstract class Controller : IController
    {
	public Controller()
	{
	    ModelState = new Model();
	}

	public Model ModelState { get; }
	public string Name => GetType().Name
	    .Replace(MvcContext.Get.ControllersSuffix, string.Empty);
	public IHttpRequest Request { get; set; }

	private IRenderable BuildView(string fullyQualifiedViewName, ViewModel viewModel)
	{
	    if (viewModel != null)
	    {
		viewModel.PageTitle = Name;
		viewModel.IsAuthenticated = Request.Session
		    .Parameters[Constants.SessionAuthenticationKey].ToString();
	    }
	    IRenderable view = new View(fullyQualifiedViewName, viewModel);
	    return view;
	}

	protected IViewable View(ViewModel viewModel = null, [CallerMemberName] string action = "")
	{
	    string fullyQualifiedViewName = MvcContext.Get.AppPath
		+ Constants.FolderSeparator + MvcContext.Get.ViewsFolderName
		+ Constants.FolderSeparator + Name
		+ Constants.FolderSeparator + action
		+ Constants.HtmlFileExtension;
	    IRenderable view = BuildView(fullyQualifiedViewName, viewModel);
	    IViewable viewResult = new ViewResult(view);
	    return viewResult;
	}

	protected IRedirectable RedirectTo(string redirectUrl)
	{
	    IRedirectable redirectResult = new RedirectResult(redirectUrl);
	    return redirectResult;
	}

	protected IViewable Unauthorized(ViewModel viewModel = null, [CallerMemberName] string action = "")
	{
	    string fullyQualifiedViewName = MvcContext.Get.AppPath
		+ Constants.FolderSeparator + MvcContext.Get.ViewsFolderName
		+ action + Constants.HtmlFileExtension;
	    IRenderable view = BuildView(fullyQualifiedViewName, viewModel);
	    IUnauthorized unauthorizedResult = new UnauthorizedResult(view);
	    return unauthorizedResult;
	}
    }
}
