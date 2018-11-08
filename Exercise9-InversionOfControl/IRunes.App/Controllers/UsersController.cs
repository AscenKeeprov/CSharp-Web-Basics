using System.Threading.Tasks;
using IRunes.App.Common;
using IRunes.App.Controllers.Contracts;
using IRunes.App.ViewModels;
using IRunes.Services.Contracts;
using SIS.Framework.ActionResults.Contracts;
using SIS.Framework.Attributes.Methods;
using SIS.Framework.Controllers;
using SIS.Services.Contracts;

namespace IRunes.App.Controllers
{
    public class UsersController : Controller, IUsersController
    {
	private readonly IUserService UserService;
	private readonly IEncryptionService Encryptor;

	public UsersController(IUserService userService, IEncryptionService encryptionService)
	{
	    UserService = userService;
	    Encryptor = encryptionService;
	}

	[HttpGet]
	public IActionResult Login() => View(new LoginViewModel());

	[HttpPost]
	public IActionResult Login(LoginViewModel model)
	{
	    string username = model.Username;
	    var user = UserService.GetUser(username);
	    if (user == null)
	    {
		model.Error = Constants.InvalidCredentialsError;
		return View(model);
	    }
	    string hashedPassword = Encryptor.HashPassword(model.Password);
	    if (user.Password != hashedPassword)
	    {
		model.Error = Constants.InvalidCredentialsError;
		return View(model);
	    }
	    if (!string.IsNullOrEmpty(user.FirstName))
	    {
		Request.Session.SetParameter(Constants.SessionUsernameKey, user.FirstName);
	    }
	    else
	    {
		Request.Session.SetParameter(Constants.SessionUsernameKey, user.Username);
	    }
	    Request.Session.SetParameter(Constants.SessionAuthenticationKey, true.ToString().ToLower());
	    return RedirectTo(Constants.HomeViewRoute);
	}

	[HttpGet]
	public IActionResult Logout()
	{
	    Request.Session.SetParameter(Constants.SessionAuthenticationKey, false.ToString().ToLower());
	    return RedirectTo(Constants.HomeViewRoute);
	}

	[HttpGet]
	public IActionResult Register() => View(new LoginViewModel());

	[HttpPost]
	public IActionResult Register(RegisterViewModel model)
	{
	    Task<string> hashTask = Task.Run(() =>
	    {
		return Encryptor.HashPassword(model.Password);
	    });
	    string username = model.Username;
	    if (UserService.Exists(username))
	    {
		model.Error = string.Format(Constants.UsernameTakenError, username);
		return View(model);
	    }
	    UserService.AddUser(username, model.FirstName,
		model.LastName, model.Email, hashTask.Result);
	    return RedirectTo(Constants.LoginViewRoute);
	}
    }
}
