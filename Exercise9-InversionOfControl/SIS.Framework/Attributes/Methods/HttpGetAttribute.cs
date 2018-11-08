using SIS.HTTP.Enumerations;

namespace SIS.Framework.Attributes.Methods
{
    public class HttpGetAttribute : HttpMethodAttribute
    {
	public override bool IsValid(HttpRequestMethod requestMethod)
	{
	    if (requestMethod.Equals(HttpRequestMethod.GET)) return true;
	    return false;
	}
    }
}
