using System;
using IRunes.App.Common;
using IRunes.App.Controllers;
using IRunes.App.Controllers.Contracts;
using IRunes.App.Data;
using IRunes.App.Services;
using IRunes.App.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SIS.HTTP.Enums;
using SIS.HTTP.Sessions;
using SIS.HTTP.Sessions.Contracts;
using SIS.WebServer;
using SIS.WebServer.Routing;
using SIS.WebServer.Routing.Contracts;

namespace IRunes.App
{
    public class Launcher
    {
	public static void Main()
	{
	    IServiceProvider services = ConfigureServices();
	    IServerRoutingTable routes = RegisterRoutes(services);
	    Server server = new Server(8000, services);
	    server.Run();
	}

	private static IServiceProvider ConfigureServices()
	{
	    var serviceCollection = new ServiceCollection();
	    serviceCollection.AddSingleton<IServerRoutingTable, ServerRoutingTable>();
	    serviceCollection.AddSingleton<IHttpSessionStorage, HttpSessionStorage>();
	    serviceCollection.AddDbContext<IRunesDbContext>(options
		=> options.UseSqlServer(IRunesDbConfig.ConnectionString)
		.UseLazyLoadingProxies(true));
	    serviceCollection.AddSingleton<IHomeController, HomeController>();
	    serviceCollection.AddSingleton<IUsersController, UsersController>();
	    serviceCollection.AddSingleton<IAlbumsController, AlbumsController>();
	    serviceCollection.AddSingleton<ITracksController, TracksController>();
	    serviceCollection.AddTransient<IEncryptionService, EncryptionService>();
	    serviceCollection.AddTransient<IDatabaseInitializationService, DatabaseInitializationService>();
	    var serviceProvider = serviceCollection.BuildServiceProvider();
	    return serviceProvider;
	}

	private static IServerRoutingTable RegisterRoutes(IServiceProvider services)
	{
	    var homeController = (HomeController)services.GetService(typeof(IHomeController));
	    var usersController = (UsersController)services.GetService(typeof(IUsersController));
	    var albumsController = (AlbumsController)services.GetService(typeof(IAlbumsController));
	    var tracksController = (TracksController)services.GetService(typeof(ITracksController));
	    var srt = (ServerRoutingTable)services.GetRequiredService(typeof(IServerRoutingTable));
	    srt.Routes[HttpRequestMethod.Get]["/"] = request => homeController.Index(request);
	    srt.Routes[HttpRequestMethod.Get][Constants.HomeViewRoute] = request => homeController.Index(request);
	    srt.Routes[HttpRequestMethod.Get][Constants.LoginViewRoute] = request
		=> usersController.LoginGet(request);
	    srt.Routes[HttpRequestMethod.Post][Constants.LoginViewRoute] = request
		=> usersController.LoginPost(request);
	    srt.Routes[HttpRequestMethod.Get][Constants.LogoutViewRoute] = request
		=> usersController.Logout(request);
	    srt.Routes[HttpRequestMethod.Get][Constants.RegisterViewRoute] = request
		=> usersController.RegisterGet(request);
	    srt.Routes[HttpRequestMethod.Post][Constants.RegisterViewRoute] = request
		=> usersController.RegisterPost(request);
	    srt.Routes[HttpRequestMethod.Get][Constants.AlbumsViewRoute] = request
		=> albumsController.BrowseAll(request);
	    srt.Routes[HttpRequestMethod.Get][Constants.CreateAlbumViewRoute] = request
		=> albumsController.CreateGet(request);
	    srt.Routes[HttpRequestMethod.Post][Constants.CreateAlbumViewRoute] = request
		=> albumsController.CreatePost(request);
	    srt.Routes[HttpRequestMethod.Get][Constants.AlbumDetailsViewRoute] = request
		=> albumsController.BrowseOne(request);
	    srt.Routes[HttpRequestMethod.Get][Constants.CreateTrackViewRoute] = request
		=> tracksController.CreateGet(request);
	    srt.Routes[HttpRequestMethod.Post][Constants.CreateTrackViewRoute] = request
		=> tracksController.CreatePost(request);
	    srt.Routes[HttpRequestMethod.Get][Constants.TrackDetailsViewRoute] = request
		=> tracksController.BrowseOne(request);
	    return srt;
	}
    }
}
