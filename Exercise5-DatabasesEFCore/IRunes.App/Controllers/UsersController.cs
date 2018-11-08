using System;
using System.Linq;
using System.Threading.Tasks;
using IRunes.App.Common;
using IRunes.App.Controllers.Contracts;
using IRunes.App.Models;
using IRunes.App.Services;
using IRunes.App.Services.Contracts;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;

namespace IRunes.App.Controllers
{
    public class UsersController : BaseController, IUsersController
    {
	private IEncryptionService Encryptor
	    => (EncryptionService)services.GetService(typeof(IEncryptionService));

	public UsersController(IServiceProvider services) : base(services) { }

	public IHttpResponse LoginGet(IHttpRequest request)
	{
	    return BuildView(request.Path, request);
	}

	public IHttpResponse LoginPost(IHttpRequest request)
	{
	    var formData = request.FormData;
	    Task<string> hashTask = Task.Run(() =>
	    {
		return Encryptor.HashPassword(formData["password"].ToString().Split('=')[1]);
	    });
	    string username = formData["username"].ToString().Split('=')[1];
	    var user = Context.Users.SingleOrDefault(u =>
	    (u.Username == username || u.Email == username) && u.Password == hashTask.Result);
	    if (user == null)
	    {
		viewBag[Constants.PageErrorPlaceholder] = string
		    .Format(Constants.ErrorMessage, Constants.InvalidCredentialsError);
		return BuildView(request.Path, request);
	    }
	    if (!string.IsNullOrEmpty(user.FirstName))
		request.Session.SetParameter(Constants.SessionUsernameKey, user.FirstName);
	    else request.Session.SetParameter(Constants.SessionUsernameKey, user.Username);
	    request.Session.SetParameter(Constants.SessionAuthenticationKey, true.ToString().ToLower());
	    return BuildView(Constants.HomeViewRoute, request, HttpResponseStatusCode.SeeOther);
	}

	public IHttpResponse Logout(IHttpRequest request)
	{
	    request.Session.SetParameter(Constants.SessionUsernameKey, Constants.SessionDefaultUsername);
	    request.Session.SetParameter(Constants.SessionAuthenticationKey, false.ToString().ToLower());
	    return BuildView(Constants.HomeViewRoute, request, HttpResponseStatusCode.SeeOther);
	}

	public IHttpResponse RegisterGet(IHttpRequest request)
	{
	    return BuildView(request.Path, request);
	}

	public IHttpResponse RegisterPost(IHttpRequest request)
	{
	    var formData = request.FormData;
	    Task<string> hashTask = Task.Run(() =>
	    {
		return Encryptor.HashPassword(formData["password"].ToString().Split('=')[1]);
	    });
	    string username = formData["username"].ToString().Split('=')[1];
	    if (Context.Users.Any(u => u.Username == username || username == Constants.SessionDefaultUsername))
	    {
		viewBag[Constants.PageErrorPlaceholder] = string
		    .Format(Constants.ErrorMessage, string
		    .Format(Constants.UsernameTakenError, username));
		return BuildView(request.Path, request);
	    }
	    string firstName = formData["firstName"].ToString().Split('=')[1];
	    string lastName = formData["lastName"].ToString().Split('=')[1];
	    string email = formData["email"].ToString().Split('=')[1];
	    var user = new User()
	    {
		Username = username,
		FirstName = firstName,
		LastName = lastName,
		Email = email,
		Password = hashTask.Result
	    };
	    Context.Users.Add(user);
	    Context.SaveChanges();
	    return BuildView(Constants.LoginViewRoute, request, HttpResponseStatusCode.SeeOther);
	}
    }
}
