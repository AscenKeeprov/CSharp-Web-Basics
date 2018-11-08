using System;
using System.Collections.Generic;
using System.Linq;
using SIS.HTTP.Common;
using SIS.HTTP.Sessions.Contracts;

namespace SIS.HTTP.Sessions
{
    public class HttpSession : IHttpSession
    {
	private readonly IDictionary<string, object> parameters;

	public HttpSession()
	{
	    Id = Guid.NewGuid().ToString();
	    parameters = new Dictionary<string, object>()
	    {
		{ Constants.SessionAuthenticationKey, false.ToString().ToLower() }
	    };
	}

	public HttpSession(string id) : this()
	{
	    Id = id;
	}

	public string Id { get; }
	public IReadOnlyDictionary<string, object> Parameters
	    => (IReadOnlyDictionary<string, object>)parameters;

	public void AddParameter(string name, object parameter)
	{
	    if (parameter != null && !ContainsParameter(name))
		parameters.Add(name, parameter);
	}

	public void ClearParameters()
	{
	    parameters.Clear();
	}

	public bool ContainsParameter(string name)
	{
	    return parameters.ContainsKey(name);
	}

	public object GetParameter(string name)
	{
	    if (string.IsNullOrEmpty(name))
		throw new ArgumentNullException("Session parameter");
	    object parameter = parameters.FirstOrDefault(p => p.Key == name).Value;
	    return parameter;
	}

	public void SetParameter(string name, object value)
	{
	    if (string.IsNullOrWhiteSpace(name) || value == null)
		throw new ArgumentNullException();
	    if (!parameters.ContainsKey(name))
		AddParameter(name, value);
	    parameters[name] = value;
	}
    }
}
