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
	    RegisterAppData();
	    RegisterControllersData();
	    RegisterResourcesData();
	    RegisterViewsData();
	    server.Run();
	}

	private static void RegisterAppData()
	{
	    Assembly appAssembly = Assembly.GetEntryAssembly();
	    MvcContext.Get.AppName = appAssembly.GetName().Name;
	    Match appPathMatch = Regex.Match(
		appAssembly.EscapedCodeBase,
		Constants.AppPathPattern);
	    if (appPathMatch.Success)
	    {
		MvcContext.Get.AppPath = appPathMatch.Groups["appPath"].Value;
	    }
	}

	private static void RegisterControllersData()
	{
	    MvcContext.Get.ControllersFolderName = "Controllers";
	    MvcContext.Get.ControllersSuffix = "Controller";
	}

	private static void RegisterResourcesData()
	{
	    MvcContext.Get.ResourcesFolderName = "Resources";
	}

	private static void RegisterViewsData()
	{
	    MvcContext.Get.ErrorTemplateFile = "Error.html";
	    MvcContext.Get.HtmlTemplateFile = "_Layout.html";
	    MvcContext.Get.ViewsFolderName = "Views";
	    MvcContext.Get.ViewModelsFolderName = "ViewModels";
	}
    }
}
