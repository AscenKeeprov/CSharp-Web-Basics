using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using IRunes.App.Common;
using IRunes.App.Data;
using IRunes.App.Exceptions;
using IRunes.App.Services;
using IRunes.App.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using SIS.HTTP.Enumerations;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Results;

namespace IRunes.App.Controllers
{
    public abstract class BaseController
    {
	private string AppLocation => Assembly.GetEntryAssembly().Location;
	private string AppRootDir => AppLocation.Substring(0, AppLocation.IndexOf("\\bin\\"));
	protected readonly IServiceProvider services;
	protected IDatabaseInitializationService DbInitializer => (DatabaseInitializationService)services
	    .GetService(typeof(IDatabaseInitializationService));
	protected IRunesDbContext Context => (IRunesDbContext)services
	    .GetRequiredService(typeof(IRunesDbContext));
	protected IDictionary<string, string> viewBag;
	protected string Name => GetType().Name.Replace("Controller", string.Empty);

	public BaseController()
	{
	    viewBag = new Dictionary<string, string>();
	}

	protected BaseController(IServiceProvider services) : this()
	{
	    this.services = services;
	    bool isDbInitialized = DbInitializer.InitializeDatabaseAsync(Context).Result;
	    if (!isDbInitialized) throw new DatabaseInitializationException();
	}

	protected IHttpResponse BuildView(string viewName, IHttpRequest request,
	    HttpResponseStatusCode responseStatus = HttpResponseStatusCode.OK)
	{
	    if (responseStatus == HttpResponseStatusCode.BadRequest)
	    {
		return new BadRequestResult();
	    }
	    string htmlLayoutPath = AppRootDir.Replace("\\", "/") + Constants.HtmlLayout;
	    if (!File.Exists(htmlLayoutPath))
	    {
		return new NotFoundResult("Page", "layout");
	    }
	    var htmlLayoutLoadTask = File.ReadAllTextAsync(htmlLayoutPath);
	    string htmlViewPath = AppRootDir.Replace("\\", "/") + Constants.HtmlViewTemplate;
	    string htmlViewFile = string.Format(htmlViewPath, viewName);
	    if (!File.Exists(htmlViewFile))
	    {
		return new NotFoundResult("View", viewName);
	    }
	    var htmlBodyLoadTask = File.ReadAllTextAsync(htmlViewFile);
	    foreach (var sessionParameter in request.Session.Parameters)
	    {
		viewBag[sessionParameter.Key] = sessionParameter.Value.ToString();
	    }
	    string pageTitle = viewName.Split('/', StringSplitOptions.RemoveEmptyEntries)[0];
	    string htmlLayout = htmlLayoutLoadTask.Result
		.Replace(Constants.PageTitlePlaceholder, pageTitle);
	    if (viewBag.ContainsKey(Constants.ErrorMessagePlaceholder))
	    {
		string errorTemplatePath = AppRootDir.Replace("\\", "/") + Constants.ErrorMessageTemplate;
		if (!File.Exists(errorTemplatePath))
		{
		    return new NotFoundResult("Error", "template");
		}
		var errorTemplateLoadTask = File.ReadAllTextAsync(errorTemplatePath);
		string errorMessage = viewBag[Constants.ErrorMessagePlaceholder];
		htmlLayout = htmlLayout.Replace(Constants.ErrorTemplatePlaceholder,
		    $"{errorTemplateLoadTask.Result}{Environment.NewLine}");
		htmlLayout = htmlLayout.Replace(Constants.ErrorMessagePlaceholder, errorMessage);
		viewBag.Remove(Constants.ErrorMessagePlaceholder);
	    }
	    else htmlLayout = htmlLayout.Replace(Constants.ErrorTemplatePlaceholder, string.Empty);
	    string content = htmlLayout.Replace(Constants.PageBodyPlaceholder, htmlBodyLoadTask.Result);
	    foreach (var item in viewBag)
	    {
		content = content.Replace(item.Key, item.Value);
	    }
	    if (responseStatus == HttpResponseStatusCode.SeeOther)
	    {
		return new RedirectResult(viewName, content);
	    }
	    return new HtmlResult(content, responseStatus);
	}
    }
}
