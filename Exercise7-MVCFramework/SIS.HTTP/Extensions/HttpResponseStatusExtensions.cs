using SIS.HTTP.Enumerations;

namespace SIS.HTTP.Extensions
{
    public static class HttpResponseStatusExtensions
    {
	public static string GetResponseLine(this HttpResponseStatusCode statusCode)
	{
	    string responseLine = $"{(int)statusCode} {statusCode}";
	    return responseLine;
	}
    }
}
