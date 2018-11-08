using System.IO;
using SIS.Framework.ActionResults.Contracts;

namespace SIS.Framework.Views
{
    public class View : IRenderable
    {
	private readonly string fullyQualifiedTemplateName;

	public View(string fullyQualifiedTemplateName)
	{
	    this.fullyQualifiedTemplateName = fullyQualifiedTemplateName;
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

	public string Render()
	{
	    string fullHtml = ReadFile(fullyQualifiedTemplateName);
	    return fullHtml;
	}
    }
}
