using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

	public void AddCookie(IHttpCookie cookie)
	{
	    if (cookie != null)
	    {
		if (!ContainsCookie(cookie.Name))
		{
		    cookies.Add(cookie.Name, cookie);
		}
	    }
	}

	public bool ContainsCookie(string name)
	{
	    return cookies.ContainsKey(name);
	}

	public IHttpCookie GetCookie(string name)
	{
	    return cookies.SingleOrDefault(c => c.Key == name).Value;
	}

	public void SetCookie(IHttpCookie cookie)
	{
	    if (cookie != null)
	    {
		if (ContainsCookie(cookie.Name))
		{
		    cookies[cookie.Name] = cookie;
		}
	    }
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
	    StringBuilder cookiesInfo = new StringBuilder();
	    cookiesInfo.Append(string.Join("; ", cookies.Values));
	    cookiesInfo.Append(Environment.NewLine);
	    return cookiesInfo.ToString();
	}
    }
}
