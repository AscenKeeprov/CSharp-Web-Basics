using System.IO;
using System.Text.RegularExpressions;
using SIS.Framework.Common;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Api.Contracts;
using SIS.WebServer.Results;

namespace SIS.Framework.Routers
{
    public class ResourceRouter : IResourceHandler
    {
	public IHttpResponse Handle(IHttpRequest request)
	{
	    var resourceInfo = Regex.Match(request.Path, Constants.ResourcePattern, RegexOptions.IgnoreCase);
	    string resourceName = resourceInfo.Groups["fileName"].Value;
	    string resourceType = resourceInfo.Groups["fileType"].Value;
	    string resourcePath = MvcContext.Get.AppPath
		+ Constants.FolderSeparator + MvcContext.Get.ResourcesFolderName
		+ Constants.FolderSeparator + resourceType
		+ Constants.FolderSeparator + resourceName;
	    if (File.Exists(resourcePath))
	    {
		byte[] content = File.ReadAllBytes(resourcePath);
		return new InlineResourceResult(content);
	    }
	    else return new NotFoundResult("File", resourceName);
	}
    }
}
