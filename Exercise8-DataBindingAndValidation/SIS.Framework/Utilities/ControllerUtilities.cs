using SIS.Framework.Controllers;

namespace SIS.Framework.Utilities
{
    public static class ControllerUtilities
    {
	public static string GetControllerName(Controller controller)
	    => controller.GetType().Name
	    .Replace(MvcContext.Get.ControllersSuffix, string.Empty);

	//TODO: CONFIRM SLASHES COUNT & ORIENTATION
	//TODO: ADD .html EXTENSION ??? WHAT ABOUT OTHER FILE TYPES ?
	public static string GetFullyQualifiedViewName(string controller, string action)
	    => string.Format("{0}/{1}/{2}/{3}.html",
		MvcContext.Get.AppPath,
		MvcContext.Get.ViewsFolder,
		controller,
		action);
    }
}
