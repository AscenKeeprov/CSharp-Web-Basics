namespace SIS.HTTP.Sessions.Contracts
{
    public interface IHttpSessionStorage
    {
	IHttpSession GetSession(string id);
    }
}
