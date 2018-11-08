using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SIS.HTTP.Common;
using SIS.HTTP.Enums;
using SIS.HTTP.Exceptions;
using SIS.HTTP.Headers;
using SIS.HTTP.Headers.Contracts;
using SIS.HTTP.Requests.Contracts;

namespace SIS.HTTP.Requests
{
    public class HttpRequest : IHttpRequest
    {
	public HttpRequest(string requestString)
	{
	    FormData = new Dictionary<string, object>();
	    QueryData = new Dictionary<string, object>();
	    Headers = new HttpHeaderCollection();
	    ParseRequest(requestString);
	}

	public string Path { get; private set; }
	public string Url { get; private set; }
	public Dictionary<string, object> FormData { get; }
	public Dictionary<string, object> QueryData { get; }
	public IHttpHeaderCollection Headers { get; }
	public HttpRequestMethod RequestMethod { get; private set; }

	private void ParseRequest(string requestString)
	{
	    string[] requestComponents = requestString.Split(Environment.NewLine);
	    string[] requestLine = requestComponents[0]
		.Split(' ', StringSplitOptions.RemoveEmptyEntries);
	    if (!IsValidRequestLine(requestLine)) throw new BadRequestException();
	    ParseRequestMethod(requestLine[0]);
	    ParseRequestUrl(requestLine[1]);
	    string[] requestHeaders = requestComponents.Skip(1).ToArray();
	    string requestBody = requestComponents.Last();
	    if (!requestBody.Contains(": "))
	    {
		requestHeaders = requestHeaders.Take(requestHeaders.Length - 1).ToArray();
		ParseFormData(requestBody);
	    }
	    ParseHeaders(requestHeaders);
	}

	private bool IsValidRequestLine(string[] requestLine)
	{
	    if (requestLine.Length != 3) return false;
	    if (!requestLine[2].Equals(GlobalConstants.HttpOneProtocolFragment)) return false;
	    return true;
	}

	private void ParseRequestMethod(string requestMethod)
	{
	    Enum.TryParse(typeof(HttpRequestMethod), requestMethod, true, out object method);
	    if (method == null) throw new BadRequestException();
	    RequestMethod = (HttpRequestMethod)method;
	}

	private void ParseRequestUrl(string requestUrl)
	{
	    if (!IsValidRequestUrl(requestUrl, out Match url))
		throw new BadRequestException();
	    Url = url.Value;
	    Path = url.Groups["path"].Value;
	    string requestQuery = url.Groups["query"].Value;
	    ParseQueryData(requestQuery);
	    string requestFragment = url.Groups["fragment"].Value;
	}

	private bool IsValidRequestUrl(string requestUrl, out Match urlComponents)
	{
	    urlComponents = Regex.Match(requestUrl, GlobalConstants.UrlPattern);
	    return urlComponents.Success;
	}

	private void ParseRequestParameters(string requestQuery, string requestBody)
	{
	    ParseQueryData(requestQuery);
	    ParseFormData(requestBody);
	}

	private void ParseQueryData(string requestQuery)
	{
	    if (IsValidRequestQuery(requestQuery))
	    {
		string[] queryParameters = requestQuery.TrimStart('?').Split('&');
		foreach (var queryParameter in queryParameters)
		{
		    string[] queryParameterArgs = queryParameter.Split('=');
		    string queryParameterKey = queryParameterArgs[0];
		    QueryData.Add(queryParameterKey, queryParameter);
		}
	    }
	}

	private bool IsValidRequestQuery(string requestQuery)
	{
	    if (string.IsNullOrEmpty(requestQuery)) return false;
	    int queryParametersCount = requestQuery
		.Split('=', StringSplitOptions.RemoveEmptyEntries).Count();
	    if (queryParametersCount == 0) return false;
	    return true;
	}

	private void ParseFormData(string requestBody)
	{
	    if (!string.IsNullOrWhiteSpace(requestBody) && requestBody.Contains('='))
	    {
		string[] bodyParameters = requestBody.Split('&');
		foreach (var bodyParameter in bodyParameters)
		{
		    string[] bodyParameterArgs = bodyParameter.Split('=');
		    string bodyParameterkey = bodyParameterArgs[0];
		    FormData.Add(bodyParameterkey, bodyParameter);
		}
	    }
	}

	private void ParseHeaders(string[] requestHeaders)
	{
	    foreach (var requestHeader in requestHeaders)
	    {
		string[] headerArgs = requestHeader.Split(": ", StringSplitOptions.RemoveEmptyEntries);
		if (headerArgs.Length == 2)
		{
		    string headerKey = headerArgs[0];
		    string headerValue = headerArgs[1];
		    IHttpHeader header = new HttpHeader(headerKey, headerValue);
		    Headers.Add(header);
		}
		if (Headers.GetHeader(GlobalConstants.HostHeaderKey) == null) throw new BadRequestException();
	    }
	}
    }
}
