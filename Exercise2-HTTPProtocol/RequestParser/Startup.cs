using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace RequestParser
{
    public class Startup
    {
	public static void Main()
	{
	    var methodsByUri = new Dictionary<Uri, HashSet<HttpMethod>>();
	    string input = string.Empty;
	    while (!(input = Console.ReadLine()).Equals("END"))
	    {
		string[] arguments = input.Split('/', StringSplitOptions.RemoveEmptyEntries);
		string path = string.Join("/", arguments.Take(arguments.Length - 1));
		Uri uri = new Uri(path, UriKind.Relative);
		HttpMethod method = new HttpMethod(arguments.Last().ToUpper());
		if (!methodsByUri.ContainsKey(uri))
		    methodsByUri.Add(uri, new HashSet<HttpMethod>());
		methodsByUri[uri].Add(method);
	    }
	    string[] request = Console.ReadLine().Split();
	    HttpMethod requestMethod = new HttpMethod(request[0].ToUpper());
	    string requestPath = string.Join("/", request.Skip(1).Take(request.Length - 2)).Trim('/');
	    Uri requestUri = new Uri(requestPath, UriKind.Relative);
	    if (!methodsByUri.ContainsKey(requestUri)
		|| !methodsByUri[requestUri].Contains(requestMethod))
		PrintResponse(HttpStatusCode.NotFound);
	    else PrintResponse(HttpStatusCode.OK);
	}

	private static void PrintResponse(HttpStatusCode statusCode)
	{
	    var response = new HttpResponseMessage(statusCode);
	    StringBuilder statusMessage = new StringBuilder();
	    string statusText = response.ReasonPhrase.Replace(" ", string.Empty);
	    statusMessage.AppendLine($"HTTP/{response.Version} {(int)statusCode} {statusText}");
	    statusMessage.AppendLine($"Content-Length: {statusText.Length}");
	    statusMessage.AppendLine($"Content-Type: text/plain{Environment.NewLine}");
	    statusMessage.AppendLine($"{statusText}");
	    Console.WriteLine(statusMessage.ToString().TrimEnd());
	}
    }
}
