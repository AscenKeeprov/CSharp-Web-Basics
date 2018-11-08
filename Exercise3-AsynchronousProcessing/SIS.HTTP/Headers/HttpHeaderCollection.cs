using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIS.HTTP.Exceptions;
using SIS.HTTP.Headers.Contracts;

namespace SIS.HTTP.Headers
{
    public class HttpHeaderCollection : IHttpHeaderCollection
    {
	private readonly Dictionary<string, IHttpHeader> headers;

	public HttpHeaderCollection()
	{
	    headers = new Dictionary<string, IHttpHeader>();
	}

	public void Add(IHttpHeader header)
	{
	    if (!ContainsHeader(header.Key))
		headers.Add(header.Key, header);
	}

	public bool ContainsHeader(string key)
	{
	    if (string.IsNullOrEmpty(key))
		throw new InvalidHeaderException(key);
	    return headers.ContainsKey(key);
	}

	public int Count()
	{
	    return headers.Count;
	}

	public IHttpHeader GetHeader(string key)
	{
	    if (string.IsNullOrEmpty(key))
		throw new InvalidHeaderException(key);
	    var header = headers.SingleOrDefault(h => h.Key == key);
	    return header.Value;
	}

	public override string ToString()
	{
	    StringBuilder headersInfo = new StringBuilder();
	    foreach (var header in headers)
	    {
		headersInfo.AppendLine(header.Value.ToString());
	    }
	    return headersInfo.ToString().TrimEnd();
	}
    }
}
