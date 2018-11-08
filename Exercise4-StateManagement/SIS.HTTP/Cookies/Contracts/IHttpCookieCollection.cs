using System.Collections.Generic;

namespace SIS.HTTP.Cookies.Contracts
{
    public interface IHttpCookieCollection : IEnumerable<IHttpCookie>
    {
	void Add(IHttpCookie cookie);
	bool ContainsCookie(string name);
	IHttpCookie GetCookie(string name);
    }
}
