using System;
using System.Reflection;
using System.Text.RegularExpressions;
using SIS.Framework.Common;
using SIS.WebServer.Contracts;

namespace SIS.Framework
{
    public static class MvcEngine
    {
	public static void Run(IServer server)
	{
	    RegisterAppPath();
	    RegisterAssemblyName();
	    RegisterControllersData();
	    RegisterModelsData();
	    RegisterResourcesData();
	    RegisterViewsData();
	    try
	    {
		server.Run();
	    }
	    catch (Exception exception)
	    {
		Console.WriteLine(exception.Message);
	    }
	}

	private static void RegisterAppPath()
	{
	    Match appPathMatch = Regex.Match(
		Assembly.GetEntryAssembly().EscapedCodeBase,
		Constants.AppPathPattern);
	    if (appPathMatch.Success)
	    {
		MvcContext.Get.AppPath = appPathMatch.Groups["appPath"].Value;
	    }
	}

	private static void RegisterAssemblyName()
	{
	    //TODO: IS THIS WHAT I NEED ???
	    MvcContext.Get.AssemblyName = Assembly
		.GetEntryAssembly().GetName().Name;
	}

	private static void RegisterControllersData()
	{
	    MvcContext.Get.ControllersFolder = "Controllers";
	    MvcContext.Get.ControllersSuffix = "Controller";
	}

	private static void RegisterModelsData()
	{
	    MvcContext.Get.ModelsFolder = "Models";
	}

	private static void RegisterResourcesData()
	{
	    MvcContext.Get.ResourcesFolder = "Resources";
	}

	private static void RegisterViewsData()
	{
	    MvcContext.Get.ViewsFolder = "Views";
	}
    }
}
