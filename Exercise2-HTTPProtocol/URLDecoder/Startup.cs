using System;
using System.Net;

namespace URLDecoder
{
    public class Startup
    {
	public static void Main()
	{
	    string encodedUrl = Console.ReadLine().Trim();
	    string decodedUrl = WebUtility.UrlDecode(encodedUrl);
	    Console.WriteLine(decodedUrl);
	}
    }
}
