using System;

namespace SIS.HTTP.Cookies.Contracts
{
    public interface IHttpCookie
    {
	DateTime Expires { get; }
	bool IsHttpOnly { get; }
	bool IsNew { get; }
	int MaxAge { get; }
	string Name { get; }
	string Value { get; }
    }
}
