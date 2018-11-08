using System;
using System.Net;
using System.Text;

namespace URLValidator
{
    public class Startup
    {
	public static void Main()
	{
	    string encodedUrl = Console.ReadLine().Trim();
	    string decodedUrl = WebUtility.UrlDecode(encodedUrl);
	    try
	    {
		if (!Uri.TryCreate(decodedUrl, UriKind.RelativeOrAbsolute, out Uri url))
		    throw new InvalidOperationException();
		if (!url.IsDefaultPort || url.Port == -1) throw new UriFormatException();
		StringBuilder urlComponents = new StringBuilder();
		urlComponents.AppendLine($"Protocol: {url.Scheme}");
		urlComponents.AppendLine($"Host: {url.Host}");
		urlComponents.AppendLine($"Port: {url.Port}");
		urlComponents.AppendLine($"Path: {url.AbsolutePath}");
		if (!string.IsNullOrWhiteSpace(url.Query))
		{
		    string urlQuery = url.Query.TrimStart('?');
		    if (string.IsNullOrWhiteSpace(urlQuery)) throw new UriFormatException();
		    urlComponents.AppendLine($"Query: {urlQuery}");
		}
		if (!string.IsNullOrWhiteSpace(url.Fragment))
		{
		    string urlFragment = url.Fragment.TrimStart('#');
		    if (string.IsNullOrWhiteSpace(urlFragment)) throw new UriFormatException();
		    urlComponents.AppendLine($"Fragment: {urlFragment}");
		}
		Console.WriteLine(urlComponents.ToString().TrimEnd());
	    }
	    catch (Exception)
	    {
		Console.WriteLine("Invalid URL");
	    }
	}
    }
}
