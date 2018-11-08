using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

	public void AddHeader(IHttpHeader header)
	{
	    if (header != null)
	    {
		if (!ContainsHeader(header.Key))
		{
		    headers.Add(header.Key, header);
		}
		else SetHeader(header);
	    }
	}

	public bool ContainsHeader(string key)
	{
	    return headers.ContainsKey(key);
	}

	public IHttpHeader GetHeader(string key)
	{
	    return headers.SingleOrDefault(h => h.Key == key).Value;
	}

	public void SetHeader(IHttpHeader header)
	{
	    if (header != null)
	    {
		if (!ContainsHeader(header.Key))
		{
		    AddHeader(header);
		}
		else headers[header.Key] = header;
	    }
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
	    return headers.GetEnumerator();
	}

	public IEnumerator<IHttpHeader> GetEnumerator()
	{
	    return headers.Select(h => h.Value).GetEnumerator();
	}

	public override string ToString()
	{
	    StringBuilder headersInfo = new StringBuilder();
	    foreach (var header in headers)
	    {
		headersInfo.Append(header.Value.ToString());
		headersInfo.Append(Environment.NewLine);
	    }
	    return headersInfo.ToString();
	}
    }
}
