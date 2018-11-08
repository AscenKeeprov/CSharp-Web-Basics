using System.Collections.Generic;

namespace SIS.Framework.Models
{
    public class ViewModel
    {
	public ViewModel()
	{
	    Data = new Dictionary<string, object>();
	}

	public IDictionary<string, object> Data { get; }

	public object this[string key]
	{
	    get => Data[key];
	    set => Data[key] = value;
	}
    }
}
