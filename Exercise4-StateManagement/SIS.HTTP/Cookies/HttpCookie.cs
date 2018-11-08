using System;
using System.Text;
using SIS.HTTP.Common;
using SIS.HTTP.Cookies.Contracts;

namespace SIS.HTTP.Cookies
{
    public class HttpCookie : IHttpCookie
    {
	public HttpCookie(string name, string value)
	{
	    Expires = DateTime.UtcNow.AddDays(GlobalConstants.HttpCookieDefaultLifetimeInDays);
	    IsHttpOnly = true;
	    IsNew = true;
	    MaxAge = GlobalConstants.HttpCookieDefaultLifetimeInDays * GlobalConstants.SecondsInDay;
	    Name = name;
	    Value = value;
	}

	public HttpCookie(string name, string value, int lifetimeInDays) : this(name, value)
	{
	    Expires = DateTime.UtcNow.AddDays(lifetimeInDays);
	    MaxAge = lifetimeInDays * GlobalConstants.SecondsInDay;
	}

	public HttpCookie(string name, string value, int lifetimeInDays, bool isNew)
	    : this(name, value, lifetimeInDays)
	{
	    IsNew = isNew;
	}

	public HttpCookie(string name, string value, int lifetimeInDays, bool isNew, bool isHttpOnly)
	    : this(name, value, lifetimeInDays, isNew)
	{
	    IsHttpOnly = isHttpOnly;
	}

	public DateTime Expires { get; }
	public bool IsHttpOnly { get; }
	public bool IsNew { get; }
	public int MaxAge { get; }
	public string Name { get; }
	public string Value { get; }

	public override string ToString()
	{
	    StringBuilder cookieInfo = new StringBuilder();
	    cookieInfo.Append($"{Name}={Value}");
	    cookieInfo.Append($"; Expires={Expires:R}");
	    cookieInfo.Append($"; Max-Age={MaxAge}");
	    if (IsHttpOnly) cookieInfo.Append($"; HttpOnly");
	    return cookieInfo.ToString();
	}
    }
}
