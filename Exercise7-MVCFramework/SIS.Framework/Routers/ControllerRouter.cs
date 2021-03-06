﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using SIS.Framework.ActionResults.Contracts;
using SIS.Framework.Attributes.Methods;
using SIS.Framework.Common;
using SIS.Framework.Controllers;
using SIS.HTTP.Enumerations;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Api.Contracts;
using SIS.WebServer.Results;

namespace SIS.Framework.Routers
{
    public class ControllerRouter : IHttpHandler
    {
	public IHttpResponse Handle(IHttpRequest request)
	{
	    if (IsResourceRequest(request, out byte[] content))
	    {
		return new InlineResourceResult(content);
	    }
	    //TODO: DOES IT MAKE SENSE TO CHECK IF request IS NULL FIRST ???
	    //TODO: USE request.Path INSTEAD ??? THINK OF RESOURCE REQUESTS
	    string[] requestUrlComponents = request.Url
		.Split('/', StringSplitOptions.RemoveEmptyEntries);
	    string controllerName = "Home";
	    string actionName = "Index";
	    if (requestUrlComponents.Length == 2)
	    {
		controllerName = requestUrlComponents[0];
		actionName = requestUrlComponents[1];
	    }
	    Controller controller = GetController(controllerName, request);
	    if (controller == null)
	    {
		return new NotFoundResult("Controller", controllerName);
	    }
	    string requestMethod = request.Method.ToString();
	    //TODO: MAKE GetMethod WORK WITH request.Method (enum)
	    MethodInfo action = GetAction(requestMethod, controller, actionName);
	    if (action == null)
	    {
		return new NotFoundResult("Controller action", actionName);
	    }
	    IHttpResponse response = PrepareResponse(controller, action);
	    return response;
	}

	private bool IsResourceRequest(IHttpRequest request, out byte[] content)
	{
	    content = new byte[0];
	    var resourceMatch = Regex.Match(request.Path,
		Constants.ResourcePattern, RegexOptions.IgnoreCase);
	    if (resourceMatch.Success)
	    {
		string resourceName = resourceMatch.Groups["fileName"].Value;
		string resourceType = resourceMatch.Groups["fileType"].Value;
		string resourcePath = $"{MvcContext.Get.AppPath}/{MvcContext.Get.ResourcesFolder}/{resourceType}/{resourceName}";
		if (File.Exists(resourcePath))
		{
		    content = File.ReadAllBytes(resourcePath);
		    return true;
		}
		else return false;
	    }
	    else return false;
	}

	//TODO: SHOULD THIS REALLY BE STATIC OR IS IT A TYPO ???
	//TODO: SUBSTITUTE WITH ControllerFactory ENTIRELY ???
	private /*static */Controller GetController(string controllerName, IHttpRequest request)
	{
	    //TODO: ADD A SECOND NULL CHECK FOR REQUEST ???
	    //      SKIP IF DONE EARLIER OR NOT NEEDED AT ALL
	    if (string.IsNullOrEmpty(controllerName)/* || request == null*/) return null;
	    //TODO: IS THERE A BETTER WAY TO PARSE THE TYPE NAME ???
	    string controllerTypeName = string.Format(
		"{0}.{1}.{2}{3}, {0}",
		MvcContext.Get.AssemblyName,
		MvcContext.Get.ControllersFolder,
		controllerName,
		MvcContext.Get.ControllersSuffix);
	    Type controllerType = Type.GetType(controllerTypeName, true, true);
	    //TODO: SWITCH TO OVERLOAD №3 FOR Activator.CreateInstance ONCE DEPENDENCY INJECTION IS IN PLACE
	    var controller = (Controller)Activator.CreateInstance(controllerType);
	    if (controller != null) controller.Request = request;
	    return controller;
	}

	private MethodInfo GetAction(string requestMethod, Controller controller, string actionName)
	{
	    //TODO: CLEAN UP
	    //MethodInfo method = null;
	    if (controller == null) return null;
	    var suitableActions = GetSuitableActions(controller, actionName);
	    if (!suitableActions.Any()) return null;
	    foreach (var suitableAction in suitableActions)
	    {
		#region
		//TODO: REVERT TO ORIGINAL IMPLEMENTATION IF MINE DOES NOT WORK
		//var httpMethodAttributes = suitableAction
		//    .GetCustomAttributes()
		//    .Where(ca => ca is HttpMethodAttribute)
		//    .Cast<HttpMethodAttribute>();
		var httpMethodAttributes = suitableAction
		    .GetCustomAttributes<HttpMethodAttribute>();
		#endregion
		//TODO: WHAT IS THE MEANING OF THIS CHECK ???
		//	WHY CREATE HttpGetMethodAttribute IF WE ARE GOING TO DO THIS ??????
		//	IF YOU FORGOT TO APPLY AN ATTRIBUTE THEN GO BACK AND DO IT !!!
		if (!httpMethodAttributes.Any()
		    && requestMethod.ToUpper().Equals("GET"))
		{//TODO: WHY ONLY "GET" OR WHY DO THIS SECOND CHECK AT ALL ???
		    return suitableAction;
		}
		foreach (var httpMethodAttribute in httpMethodAttributes)
		{
		    if (httpMethodAttribute.IsValid(requestMethod))
		    {
			return suitableAction;
		    }
		}
	    }
	    return null;
	}

	private IEnumerable<MethodInfo> GetSuitableActions(Controller controller, string actionName)
	{
	    if (controller == null) return new MethodInfo[0];
	    var suitableActions = controller.GetType().GetMethods()
		.Where(m => m.Name.ToUpper() == actionName.ToUpper());
	    return suitableActions;
	}

	private IHttpResponse PrepareResponse(Controller controller, MethodInfo action)
	{
	    var actionResult = (IActionResult)action.Invoke(controller, null);
	    string content = actionResult.Invoke();
	    if (actionResult is IViewable)
	    {
		return new HtmlResult(content, HttpResponseStatusCode.OK);
	    }
	    else if (actionResult is IRedirectable)
	    {
		var locationHeader = controller.Request.Headers
		    .FirstOrDefault(h => h.Key == "Location");
		string location = "/";
		if (locationHeader != null) location = locationHeader.Value;
		//TODO: FIX CONFLICT BETWEEN SIS.WebServer.Results AND SIS.Framework.ActionResults
		//	RIGHT NOW THIS IS NOT WORKING PROPERLY !!!
		return new RedirectResult(location, content);
	    }
	    else
	    {
		throw new InvalidOperationException("View type not supported.");
	    }
	}
    }
}
