using System.Collections.Generic;

namespace SIS.HTTP.Sessions.Contracts
{
    public interface IHttpSession
    {
	string Id { get; }
	IReadOnlyDictionary<string, object> Parameters { get; }
	void AddParameter(string name, object parameter);
	void ClearParameters();
	bool ContainsParameter(string name);
	object GetParameter(string name);
	void SetParameter(string name, object value);
    }
}
