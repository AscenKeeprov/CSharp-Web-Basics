using System;
using System.Collections.Generic;
using System.IO;
using IRunes.App.Common;
using IRunes.App.Data;
using IRunes.App.Exceptions;
using IRunes.App.Services;
using IRunes.App.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Results;

namespace IRunes.App.Controllers
{
    public abstract class BaseController
    {
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
	    HttpResponseStatusCode responseStatus = HttpResponseStatusCode.Ok)
	{
	    var htmlLayoutLoadTask = File.ReadAllTextAsync(Constants.HtmlLayoutPath);
	    var htmlBodyLoadTask = File.ReadAllTextAsync(string.Format(Constants.HtmlBodyPath, viewName));
	    foreach (var sessionParameter in request.Session.Parameters)
	    {
		    viewBag[sessionParameter.Key] = sessionParameter.Value.ToString();
	    }
	    string pageTitle = viewName.Split('/', StringSplitOptions.RemoveEmptyEntries)[0];
	    string htmlLayout = htmlLayoutLoadTask.Result
		.Replace(Constants.PageTitlePlaceholder, pageTitle);
	    if (viewBag.ContainsKey(Constants.PageErrorPlaceholder))
	    {
		htmlLayout = htmlLayout.Replace(Constants.PageBodyPlaceholder,
			$"{viewBag[Constants.PageErrorPlaceholder]}\r\n{Constants.PageBodyPlaceholder}");
		viewBag.Remove(Constants.PageErrorPlaceholder);
	    }
	    string content = htmlLayout.Replace(Constants.PageBodyPlaceholder, htmlBodyLoadTask.Result);
	    foreach (var item in viewBag)
	    {
		content = content.Replace(item.Key, item.Value);
	    }
	    IHttpResponse response = null;
	    switch (responseStatus)
	    {
		case HttpResponseStatusCode.Ok:
		    response = new HtmlResult(content, responseStatus);
		    break;
		case HttpResponseStatusCode.Created:
		    break;
		case HttpResponseStatusCode.Found:
		    break;
		case HttpResponseStatusCode.SeeOther:
		    response = new RedirectResult(viewName, content);
		    break;
		case HttpResponseStatusCode.BadRequest:
		    break;
		case HttpResponseStatusCode.Unauthorized:
		    break;
		case HttpResponseStatusCode.Forbidden:
		    break;
		case HttpResponseStatusCode.NotFound:
		    break;
		case HttpResponseStatusCode.InternalServerError:
		    break;
	    }
	    return response;
	}
    }
}
