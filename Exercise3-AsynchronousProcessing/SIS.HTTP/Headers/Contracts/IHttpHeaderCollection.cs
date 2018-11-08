namespace SIS.HTTP.Headers.Contracts
{
    public interface IHttpHeaderCollection
    {
	void Add(IHttpHeader header);
	bool ContainsHeader(string key);
	int Count();
	IHttpHeader GetHeader(string key);
    }
}
