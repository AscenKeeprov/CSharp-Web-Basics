using SIS.HTTP.Enumerations;

namespace SIS.Framework.Attributes.Methods
{
    public class HttpPostAttribute : HttpMethodAttribute
    {
	public override bool IsValid(HttpRequestMethod requestMethod)
	{
	    if (requestMethod.Equals(HttpRequestMethod.POST)) return true;
	    return false;
	}
    }
}
