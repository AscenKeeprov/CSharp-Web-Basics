using System;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SIS.HTTP.Cookies;
using SIS.HTTP.Enumerations;
using SIS.HTTP.Requests;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses;
using SIS.HTTP.Responses.Contracts;
using SIS.HTTP.Sessions;
using SIS.HTTP.Sessions.Contracts;
using SIS.WebServer.Common;
using SIS.WebServer.Contracts;
using SIS.WebServer.Results;
using SIS.WebServer.Routing;
using SIS.WebServer.Routing.Contracts;

namespace SIS.WebServer
{
    public class ConnectionHandler : IConnectionHandler
    {
	private string AppLocation => Assembly.GetEntryAssembly().Location;
	private string AppRootDir => AppLocation.Substring(0, AppLocation.IndexOf("\\bin\\"));
	private const int DataBufferSize = 1024;
	private readonly Socket client;
	private readonly IServiceProvider services;
	private IServerRoutingTable ServerRoutingTable => (ServerRoutingTable)services
	    .GetService(typeof(IServerRoutingTable));
	private IHttpSessionStorage SessionStorage => (HttpSessionStorage)services
	    .GetService(typeof(IHttpSessionStorage));

	public ConnectionHandler(Socket client, IServiceProvider services)
	{
	    this.client = client;
	    this.services = services;
	}

	public async Task ProcessRequestAsync()
	{
	    var httpRequest = await ReadRequestAsync();
	    if (httpRequest != null)
	    {
		SetRequestSession(httpRequest);
		var httpResponse = HandleRequest(httpRequest);
		SetResponseSession(httpRequest, httpResponse);
		await RenderResponseAsync(httpResponse);
	    }
	    client.Shutdown(SocketShutdown.Both);
	}

	private async Task<bool> IsConnectedAsync(Socket client)
	{
	    bool blockingState = client.Blocking;
	    try
	    {
		var ping = new ArraySegment<byte>(new byte[0]);
		client.Blocking = false;
		await client.SendAsync(ping, SocketFlags.None);
		return true;
	    }
	    catch (SocketException se)
	    {
		if (se.NativeErrorCode.Equals(10035)) return true;
		else return false;
	    }
	    finally { client.Blocking = blockingState; }
	}

	private async Task<IHttpRequest> ReadRequestAsync()
	{
	    StringBuilder requestString = new StringBuilder();
	    var dataBuffer = new ArraySegment<byte>(new byte[DataBufferSize]);
	    while (true)
	    {
		int bytesReceived = await client.ReceiveAsync(dataBuffer, SocketFlags.None);
		if (bytesReceived == 0) break;
		string data = Encoding.UTF8.GetString(dataBuffer.Array, 0, bytesReceived);
		requestString.Append(data);
		if (bytesReceived < DataBufferSize) break;
	    }
	    if (requestString.Length == 0) return null;
	    var request = new HttpRequest(requestString.ToString());
	    return request;
	}

	private void SetRequestSession(IHttpRequest request)
	{
	    if (!request.Cookies.ContainsCookie(Constants.HttpSessionKey))
	    {
		request.Session = new HttpSession();
		request.Cookies.AddCookie(new HttpCookie(Constants.HttpSessionKey, request.Session.Id));
	    }
	    if (request.Session == null)
	    {
		string sessionId = request.Cookies.GetCookie(Constants.HttpSessionKey).Value;
		request.Session = SessionStorage.GetSession(sessionId);
	    }
	    SessionStorage.AddSession(request.Session);
	}

	private IHttpResponse HandleRequest(IHttpRequest request)
	{
	    if (!ServerRoutingTable.ContainsRoute(request))
	    {
		if (IsResourceRequest(request, out byte[] content))
		{
		    return new InlineResourceResult(content);
		}
		return new HttpResponse(HttpResponseStatusCode.NotFound);
	    }
	    return ServerRoutingTable.Routes[request.Method][request.Path].Invoke(request);
	}

	private bool IsResourceRequest(IHttpRequest request, out byte[] content)
	{
	    content = new byte[0];
	    var resourceMatch = Regex.Match(request.Path, Constants.ResourcePattern, RegexOptions.IgnoreCase);
	    if (resourceMatch.Success)
	    {
		string resourceName = resourceMatch.Groups["fileName"].Value;
		string resourceType = resourceMatch.Groups["fileType"].Value;
		string resourcePath = $"{AppRootDir}{Constants.DefaultResourcesDir}{resourceType}\\{resourceName}";
		if (File.Exists(resourcePath))
		{
		    content = File.ReadAllBytes(resourcePath);
		    return true;
		}
		else return false;
	    }
	    else return false;
	}

	private void SetResponseSession(IHttpRequest request, IHttpResponse response)
	{
	    response.Session = request.Session;
	    var sessionCookie = request.Cookies.GetCookie(Constants.HttpSessionKey);
	    response.Cookies.SetCookie(sessionCookie);
	}

	private async Task RenderResponseAsync(IHttpResponse response)
	{
	    byte[] bytesToSend = response.GetBytes();
	    var dataBuffer = new ArraySegment<byte>(bytesToSend);
	    await client.SendAsync(dataBuffer, SocketFlags.None);
	}
    }
}
