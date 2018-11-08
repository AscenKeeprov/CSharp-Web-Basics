using System.Collections.Concurrent;
using SIS.HTTP.Sessions.Contracts;

namespace SIS.HTTP.Sessions
{
    public static class HttpSessionStorage
    {
	private static readonly ConcurrentDictionary<string, IHttpSession> sessions = new ConcurrentDictionary<string, IHttpSession>();

	public static IHttpSession GetSession(string id)
	{
	    var session = sessions.GetOrAdd(id, s => new HttpSession(id));
	    return session;
	}
    }
}
