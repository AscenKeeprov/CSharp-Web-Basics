﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SIS.HTTP.Common;
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
	    if (header != null && !ContainsHeader(header.Key))
		headers.Add(header.Key, header);
	}

	public bool ContainsHeader(string key)
	{
	    return headers.ContainsKey(key);
	}

	public IHttpHeader GetHeader(string key)
	{
	    if (string.IsNullOrEmpty(key))
		throw new ArgumentNullException(nameof(HttpHeader));
	    var header = headers.SingleOrDefault(h => h.Key == key).Value;
	    return header;
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
		headersInfo.Append(header.ToString() + Environment.NewLine);
	    }
	    return headersInfo.ToString().TrimEnd();
	}
    }
}
