using System.Collections.Generic;
using System.IO;
using System.Linq;
using SIS.Framework.ActionResults.Contracts;

namespace SIS.Framework.Views
{
    public class View : IRenderable
    {
	private readonly string fullyQualifiedTemplateName;
	private readonly IDictionary<string, object> viewData;

	private View()
	{
	    viewData = new Dictionary<string, object>();
	}

	public View(string fullyQualifiedTemplateName) : this()
	{
	    this.fullyQualifiedTemplateName = fullyQualifiedTemplateName;
	}

	public View(string fullyQualifiedTemplateName, IDictionary<string, object> viewData)
	    : this(fullyQualifiedTemplateName)
	{
	    //TODO: CLEAN UP
	    //this.fullyQualifiedTemplateName = fullyQualifiedTemplateName;
	    this.viewData = viewData;
	}

	public string Render()
	{
	    string fullHtml = ReadFile(fullyQualifiedTemplateName);
	    string fullHtmlWithData = PopulateFile(fullHtml);
	    return fullHtmlWithData;
	}

	//TODO: OR LoadTemplate(string fullyQualifiedTemplateName) ???
	//TODO: MAKE ASYNC ???
	private string ReadFile(string fullyQualifiedTemplateName)
	{
	    //TODO: CHECK AND REFINE
	    if (!File.Exists(fullyQualifiedTemplateName))
	    {
		string viewName = fullyQualifiedTemplateName.Substring(
		fullyQualifiedTemplateName.LastIndexOf('/'),
		fullyQualifiedTemplateName.Length - fullyQualifiedTemplateName.LastIndexOf('/'));
		throw new FileNotFoundException(string.Format("View {0} not found.", viewName));
	    }
	    //TODO: USE File.ReadAllTextAsync() INSTEAD ???
	    return File.ReadAllText(fullyQualifiedTemplateName);
	}

	private string PopulateFile(string fullHtml)
	{
	    if (viewData.Any())
	    {
		foreach (var parameter in viewData)
		{
		    fullHtml = fullHtml.Replace($"@{parameter.Key}", parameter.Value.ToString());
		}
	    }
	    return fullHtml;
	}
    }
}
