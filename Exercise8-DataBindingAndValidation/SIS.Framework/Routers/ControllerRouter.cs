using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using SIS.Framework.ActionResults.Contracts;
using SIS.Framework.Attributes.Methods;
using SIS.Framework.Controllers;
using SIS.HTTP.Enumerations;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;
using SIS.WebServer.Api.Contracts;
using SIS.WebServer.Results;

namespace SIS.Framework.Routers
{
    public class ControllerRouter : IControllerHandler
    {
	public IHttpResponse Handle(IHttpRequest request)
	{
	    //TODO: CLEAN UP
	    //   if (IsResourceRequest(request, out byte[] content))
	    //   {
	    //return new InlineResourceResult(content);
	    //   }
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
	    object[] actionParameters = GetActionParameters(controller, action, request);
	    IActionResult actionResult = (IActionResult)action.Invoke(controller, actionParameters);
	    IHttpResponse response = PrepareResponse(controller, actionResult);
	    return response;
	}

	//TODO: CLEAN UP
	//private bool IsResourceRequest(IHttpRequest request, out byte[] content)
	//{
	//    content = new byte[0];
	//    var resourceMatch = Regex.Match(request.Path,
	//	Constants.ResourcePattern, RegexOptions.IgnoreCase);
	//    if (resourceMatch.Success)
	//    {
	//	string resourceName = resourceMatch.Groups["fileName"].Value;
	//	string resourceType = resourceMatch.Groups["fileType"].Value;
	//	string resourcePath = $"{MvcContext.Get.AppPath}/{MvcContext.Get.ResourcesFolder}/{resourceType}/{resourceName}";
	//	if (File.Exists(resourcePath))
	//	{
	//	    content = File.ReadAllBytes(resourcePath);
	//	    return true;
	//	}
	//	else return false;
	//    }
	//    else return false;
	//}

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

	private object[] GetActionParameters(Controller controller, MethodInfo action, IHttpRequest request)
	{
	    ParameterInfo[] actionParameters = action.GetParameters();
	    object[] mappedActionParameters = new object[actionParameters.Length];
	    for (int p = 0; p < actionParameters.Length; p++)
	    {
		ParameterInfo currentParameter = actionParameters[p];
		if (currentParameter.ParameterType.IsPrimitive
		    || currentParameter.ParameterType == typeof(string))
		{
		    mappedActionParameters[p] = ProcessPrimitiveActionParameter(currentParameter, request);
		}
		else
		{
		    object bindingModel = ProcessBindingModelParameter(currentParameter, request);
		    controller.ModelState.IsValid = IsValidModel(bindingModel);
		    mappedActionParameters[p] = bindingModel;
		}
	    }
	    return mappedActionParameters;
	}

	private object ProcessPrimitiveActionParameter(ParameterInfo parameter, IHttpRequest request)
	{
	    object parameterValue = GetParameterFromRequestData(parameter.Name, request);
	    return Convert.ChangeType(parameterValue, parameter.ParameterType);
	}

	private object ProcessBindingModelParameter(ParameterInfo parameter, IHttpRequest request)
	{
	    Type bindingModelType = parameter.ParameterType;
	    var bindingModelInstance = Activator.CreateInstance(bindingModelType);
	    var bindingModelProperties = bindingModelType.GetProperties();
	    foreach (var property in bindingModelProperties)
	    {
		try
		{
		    object value = GetParameterFromRequestData(property.Name, request);
		    property.SetValue(bindingModelInstance, Convert.ChangeType(value, property.PropertyType));
		}
		catch
		{
		    Console.WriteLine($"Property {property.Name} could not be mapped.");
		}
	    }
	    return Convert.ChangeType(bindingModelInstance, bindingModelType);
	}

	private object GetParameterFromRequestData(string parameterName, IHttpRequest request)
	{
	    object parameterValue = null;
	    if (request.QueryData.ContainsKey(parameterName))
	    {
		parameterValue = request.QueryData[parameterName];
	    }
	    if (request.FormData.ContainsKey(parameterName))
	    {
		parameterValue = request.FormData[parameterName];
	    }
	    return parameterValue;
	}

	private bool IsValidModel(object model)
	{
	    PropertyInfo[] modelProperties = model.GetType().GetProperties();
	    foreach (var property in modelProperties)
	    {
		//TODO: REVERT TO ORIGINAL IMPLEMENTATION IF MINE DOES NOT WORK
		//var propertyValidationAttributes = property
		//    .GetCustomAttributes()
		//    .Where(ca => ca is ValidationAttribute)
		//    .Cast<ValidationAttribute>()
		//    .ToList();
		var propertyValidationAttributes = property
		    .GetCustomAttributes<ValidationAttribute>();
		foreach (var validationAttribute in propertyValidationAttributes)
		{
		    var propertyValue = property.GetValue(model);
		    if (!validationAttribute.IsValid(propertyValue))
		    {
			return false;
		    }
		}
	    }
	    return true;
	}

	private IHttpResponse PrepareResponse(Controller controller, IActionResult actionResult)
	{
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
