using System;
using System.Threading.Tasks;
using IRunes.App.Controllers;
using IRunes.App.Controllers.Contracts;
using IRunes.App.Exceptions;
using IRunes.Data;
using IRunes.Services;
using IRunes.Services.Contracts;
using SIS.Framework;
using SIS.Framework.Routers;
using SIS.HTTP.Sessions;
using SIS.HTTP.Sessions.Contracts;
using SIS.Services;
using SIS.Services.Contracts;
using SIS.WebServer;
using SIS.WebServer.Api.Contracts;
using SIS.WebServer.Contracts;

namespace IRunes.App
{
    public class Launcher
    {
	public static void Main()
	{
	    IServiceCollection services = RegisterServices();
	    var dbInitTask = InitializeDatabaseAsync(services);
	    IServer server = new Server(8000, services);
	    if (!dbInitTask.Result)
	    {
		throw new DatabaseInitializationException();
	    }
	    MvcEngine.Run(server);
	}

	private static async Task<bool> InitializeDatabaseAsync(IServiceCollection services)
	{
	    try
	    {
		var dbInitializer = services.GetService<IDbInitializationService>();
		var dbContext = services.GetDbContext<IRunesDbContext>();
		return await dbInitializer.InitializeDatabaseAsync(dbContext);
	    }
	    catch (Exception exception)
	    {
		Console.WriteLine(exception.Message);
		return false;
	    }
	}

	private static IServiceCollection RegisterServices()
	{
	    IServiceCollection services = new ServiceCollection();
	    services.RegisterService<IDbInitializationService, DbInitializationService>();
	    services.RegisterDbContext<IRunesDbContext>();
	    services.RegisterService<IHttpSessionStorage, HttpSessionStorage>();
	    services.RegisterService<IConnectionHandler, ConnectionHandler>();
	    services.RegisterService<IControllerHandler, ControllerRouter>();
	    services.RegisterService<IResourceHandler, ResourceRouter>();
	    services.RegisterService<IEnumerationService, EnumerationService>();
	    services.RegisterService<IEncryptionService, EncryptionService>();
	    services.RegisterService<IHomeController, HomeController>();
	    services.RegisterService<IUserService, UserService>();
	    services.RegisterService<IUsersController, UsersController>();
	    services.RegisterService<IAlbumService, AlbumService>();
	    services.RegisterService<IAlbumsController, AlbumsController>();
	    services.RegisterService<IAlbumTrackService, AlbumTrackService>();
	    services.RegisterService<ITrackService, TrackService>();
	    services.RegisterService<ITracksController, TracksController>();
	    return services;
	}
    }
}
