using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SIS.HTTP.Cookies.Contracts;

namespace SIS.HTTP.Cookies
{
    public class HttpCookieCollection : IHttpCookieCollection
    {
	private readonly IDictionary<string, IHttpCookie> cookies;

	public HttpCookieCollection()
	{
	    cookies = new Dictionary<string, IHttpCookie>();
	}

	public void Add(IHttpCookie cookie)
	{
	    if (cookie != null) cookies[cookie.Name] = cookie;
	}

	public bool ContainsCookie(string name)
	{
	    return cookies.ContainsKey(name);
	}

	public IHttpCookie GetCookie(string name)
	{
	    if (string.IsNullOrEmpty(name))
		throw new ArgumentNullException(nameof(HttpCookie));
	    var cookie = cookies.SingleOrDefault(c => c.Key == name).Value;
	    return cookie;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
	    return cookies.GetEnumerator();
	}

	public IEnumerator<IHttpCookie> GetEnumerator()
	{
	    return cookies.Select(c => c.Value).GetEnumerator();
	}

	public override string ToString()
	{
	    return string.Join("; ", cookies.Values);
	}
    }
}
