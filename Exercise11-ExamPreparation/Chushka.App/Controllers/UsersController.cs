using Chushka.App.Common;
using Chushka.App.ViewModels;
using Chushka.Services.Contracts;
using SIS.Framework.ActionResults;
using SIS.Framework.Attributes.Action;
using SIS.Framework.Attributes.Method;
using SIS.Framework.Security;

namespace Chushka.App.Controllers
{
    public class UsersController : BaseController
    {
	private readonly IUsersService usersService;

	public UsersController(IUsersService usersService)
	{
	    this.usersService = usersService;
	}

	[HttpGet]
	public IActionResult Login()
	{
	    return View();
	}

	[HttpPost]
	public IActionResult Login(LoginViewModel model)
	{
	    if (ModelState.IsValid != true)
	    {
		return View();
	    }
	    var user = usersService.GetUserByUsername(model.Username);
	    if (user == null || user.Password != model.Password)
	    {
		Model["Error"] = "Invalid credentials";
		return View();
	    }
	    var identity = new IdentityUser()
	    {
		Id = user.Id.ToString(),
		Username = user.Username,
		Roles = new[] { user.Role.ToString() },
		Email = user?.Email
	    };
	    SignIn(identity);
	    return RedirectToAction(Constants.HomeViewRoute);
	}

	[HttpGet]
	[Authorize]
	public IActionResult Logout()
	{
	    SignOut();
	    return RedirectToAction(Constants.HomeViewRoute);
	}

	[HttpGet]
	public IActionResult Register()
	{
	    return View();
	}

	[HttpPost]
	public IActionResult Register(RegisterViewModel model)
	{
	    if (ModelState.IsValid != true)
	    {
		return View();
	    }
	    if (usersService.Exists(model.Username))
	    {
		Model["Error"] = $"Username '{model.Username}' is already taken";
		return View();
	    }
	    if (model.Password != model.ConfirmPassword)
	    {
		Model["Error"] = $"Passwords do not match";
		return View();
	    }
	    usersService.AddUser(model.Username, model.Password, model.Email, model.FullName);
	    return RedirectToAction(Constants.LoginViewRoute);
	}
    }
}
