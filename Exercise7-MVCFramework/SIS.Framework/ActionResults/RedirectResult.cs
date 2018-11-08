using SIS.Framework.ActionResults.Contracts;

namespace SIS.Framework.ActionResults
{
    //TODO: IMPROVE THIS BASED ON SIS.WebServer.Results.RedirectResult ???
    public class RedirectResult : IRedirectable
    {
	public RedirectResult(string redirectUrl)
	{
	    RedirectUrl = redirectUrl;
	}

	public string RedirectUrl { get; }

	public string Invoke() => RedirectUrl;
    }
}
