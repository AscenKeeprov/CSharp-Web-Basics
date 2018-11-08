using System;
using Microsoft.Extensions.DependencyInjection;
using SIS.Framework;
using SIS.Framework.Routers;
using SIS.HTTP.Sessions;
using SIS.HTTP.Sessions.Contracts;
using SIS.WebServer;
using SIS.WebServer.Api.Contracts;
using SIS.WebServer.Contracts;

namespace SIS.App
{
    public class Launcher
    {
	public static void Main()
	{
	    IServiceProvider services = ConfigureServices();
	    IServer server = new Server(8000, services);
	    MvcEngine.Run(server);
	}

	private static IServiceProvider ConfigureServices()
	{
	    var serviceCollection = new ServiceCollection();
	    serviceCollection.AddSingleton<IControllerHandler, ControllerRouter>();
	    serviceCollection.AddSingleton<IResourceHandler, ResourceRouter>();
	    serviceCollection.AddSingleton<IHttpSessionStorage, HttpSessionStorage>();
	    var serviceProvider = serviceCollection.BuildServiceProvider();
	    return serviceProvider;
	}
    }
}
