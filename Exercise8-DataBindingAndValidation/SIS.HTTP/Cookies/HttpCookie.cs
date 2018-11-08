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
	    Expires = DateTime.UtcNow.AddDays(Constants.HttpCookieDefaultLifetimeInDays);
	    IsHttpOnly = true;
	    IsNew = true;
	    MaxAge = Constants.HttpCookieDefaultLifetimeInDays * Constants.SecondsInDay;
	    Name = name;
	    Value = value;
	}

	public HttpCookie(string name, string value, int lifetimeInDays) : this(name, value)
	{
	    Expires = DateTime.UtcNow.AddDays(lifetimeInDays);
	    MaxAge = lifetimeInDays * Constants.SecondsInDay;
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
	    cookieInfo.Append($"; {Constants.ExpiresCookieKey}={Expires:R}");
	    cookieInfo.Append($"; {Constants.MaxAgeCookieKey}={MaxAge}");
	    if (IsHttpOnly) cookieInfo.Append($"; {Constants.HttpOnlyCookieKey}");
	    return cookieInfo.ToString();
	}
    }
}
