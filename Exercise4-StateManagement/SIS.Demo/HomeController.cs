using System.IO;
using SIS.HTTP.Enums;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Results;

namespace SIS.Demo
{
    public class HomeController
    {
	public IHttpResponse Index()
	{
	    string content = File.ReadAllText(@"..\..\..\HTML\index.html");
	    return new HtmlResult(content, HttpResponseStatusCode.Ok);
	}
    }
}
