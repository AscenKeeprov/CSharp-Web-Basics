namespace SIS.HTTP.Sessions.Contracts
{
    public interface IHttpSessionStorage
    {
	void AddSession(IHttpSession session);
	IHttpSession GetSession(string id);
	void UpdateSession(IHttpSession session);
    }
}
