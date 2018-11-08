namespace SIS.Framework.Attributes.Methods
{
    public class HttpGetAttribute : HttpMethodAttribute
    {
	public override bool IsValid(string requestMethod)
	{
	    if (requestMethod.ToUpper().Equals("GET")) return true;
	    return false;
	}
    }
}
