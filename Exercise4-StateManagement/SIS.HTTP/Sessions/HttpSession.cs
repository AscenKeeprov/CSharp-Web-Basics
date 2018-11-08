using System;
using System.Collections.Generic;
using System.Linq;
using SIS.HTTP.Sessions.Contracts;

namespace SIS.HTTP.Sessions
{
    public class HttpSession : IHttpSession
    {
	private readonly IDictionary<string, object> parameters;

	public HttpSession(string id)
	{
	    parameters = new Dictionary<string, object>();
	    Id = id;
	}

	public string Id { get; }

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
    }
}
