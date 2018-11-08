namespace SIS.Framework.Attributes.Methods
{
    public class HttpPutAttribute : HttpMethodAttribute
    {
	public override bool IsValid(string requestMethod)
	{
	    if (requestMethod.ToUpper().Equals("PUT")) return true;
	    return false;
	}
    }
}
