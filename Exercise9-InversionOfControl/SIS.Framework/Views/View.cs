using System.IO;
using System.Threading.Tasks;
using SIS.Framework.ActionResults.Contracts;
using SIS.Framework.Common;
using SIS.Framework.Models;

namespace SIS.Framework.Views
{
    public class View : IRenderable
    {
	private readonly string fullyQualifiedViewName;
	private readonly ViewModel viewModel;

	private View()
	{
	    viewModel = new ViewModel();
	}

	public View(string fullyQualifiedViewName) : this()
	{
	    this.fullyQualifiedViewName = fullyQualifiedViewName;
	}

	public View(string fullyQualifiedViewName, ViewModel viewModel)
	    : this(fullyQualifiedViewName)
	{
	    this.viewModel = viewModel;
	}

	public string Render()
	{
	    string htmlLayoutPath = MvcContext.Get.AppPath
		+ Constants.FolderSeparator + MvcContext.Get.ViewsFolderName
		+ Constants.FolderSeparator + MvcContext.Get.HtmlTemplateFile;
	    var htmlLayoutLoadTask = ReadFileAsync(htmlLayoutPath);
	    if (viewModel != null && !string.IsNullOrEmpty(viewModel.Error))
	    {
		string htmlErrorTemplatePath = MvcContext.Get.AppPath
		+ Constants.FolderSeparator + MvcContext.Get.ViewsFolderName
		+ Constants.FolderSeparator + MvcContext.Get.ErrorTemplateFile;
		var htmlErrorTemplateLoadTask = ReadFileAsync(htmlErrorTemplatePath);
		string htmlErrorTemplate = htmlErrorTemplateLoadTask.Result
		    .Replace(Constants.HtmlErrorMessagePlaceholder, viewModel.Error);
		viewModel.Error = htmlErrorTemplate;
	    }
	    var htmlBodyLoadTask = ReadFileAsync(fullyQualifiedViewName);
	    string fullHtml = htmlLayoutLoadTask.Result
		.Replace(Constants.HtmlBodyPlaceholder, htmlBodyLoadTask.Result);
	    string fullHtmlWithData = PopulateFile(fullHtml);
	    return fullHtmlWithData;
	}

	private Task<string> ReadFileAsync(string filePath)
	{
	    if (!File.Exists(filePath))
	    {
		int fileNameStartIndex = filePath.LastIndexOf(Constants.FolderSeparator);
		if (fileNameStartIndex == -1)
		{
		    throw new FileNotFoundException("File not found.", filePath);
		}
		else
		{
		    fileNameStartIndex++;
		    string fileName = filePath.Substring(fileNameStartIndex,
			filePath.Length - fileNameStartIndex);
		    throw new FileNotFoundException("File not found.", fileName);
		}
	    }
	    return File.ReadAllTextAsync(filePath);
	}

	private string PopulateFile(string fullHtml)
	{
	    if (viewModel != null)
	    {
		var modelProperties = viewModel.GetType().GetProperties();
		foreach (var property in modelProperties)
		{
		    string propertyPlaceholder = $"@{property.Name}";
		    if (fullHtml.Contains(propertyPlaceholder))
		    {
			string propertyValue = property.GetValue(viewModel).ToString();
			fullHtml = fullHtml.Replace(propertyPlaceholder, propertyValue);
		    }
		}
	    }
	    return fullHtml;
	}
    }
}
