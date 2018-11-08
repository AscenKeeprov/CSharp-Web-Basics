using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SIS.HTTP.Common;
using SIS.HTTP.Cookies;
using SIS.HTTP.Cookies.Contracts;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses;
using SIS.HTTP.Responses.Contracts;
using SIS.HTTP.Sessions;
using SIS.WebServer.Contracts;
using SIS.WebServer.Routing.Contracts;

namespace SIS.WebServer
{
    public class ConnectionHandler : IConnectionHandler
    {
	private const int DataBufferSize = 1024;
	private readonly Socket client;
	private readonly IServerRoutingTable serverRoutingTable;

	public ConnectionHandler(Socket client, IServerRoutingTable serverRoutingTable)
	{
	    this.client = client;
	    this.serverRoutingTable = serverRoutingTable;
	}

	public async Task ProcessRequestAsync()
	{
	    var httpRequest = await ReadRequest();
	    if (httpRequest != null)
	    {
		var sessionCookie = SetRequestSession(httpRequest);
		var httpResponse = HandleRequest(httpRequest);
		SetResponseSession(httpResponse, sessionCookie);
		await PrepareResponse(httpResponse);
	    }
	    client.Shutdown(SocketShutdown.Both);
	}

	private async Task<IHttpRequest> ReadRequest()
	{
	    StringBuilder requestString = new StringBuilder();
	    var dataBuffer = new ArraySegment<byte>(new byte[DataBufferSize]);
	    while (true)
	    {
		int bytesReceived = await client.ReceiveAsync(dataBuffer, SocketFlags.None);
		if (bytesReceived == 0) break;
		string bytesAsString = Encoding.UTF8.GetString(dataBuffer.Array, 0, bytesReceived);
		requestString.Append(bytesAsString);
		if (bytesReceived < DataBufferSize) break;
	    }
	    if (requestString.Length == 0) return null;
	    return new HttpRequest(requestString.ToString());
	}

	private IHttpCookie SetRequestSession(IHttpRequest httpRequest)
	{
	    var sessionCookie = httpRequest.Cookies.GetCookie(GlobalConstants.SessionCookieKey);
	    if (sessionCookie == null)
	    {
		string sessionId = Guid.NewGuid().ToString();
		sessionCookie = new HttpCookie(GlobalConstants.SessionCookieKey, sessionId);
	    }
	    httpRequest.Session = HttpSessionStorage.GetSession(sessionCookie.Value);
	    return sessionCookie;
	}

	private IHttpResponse HandleRequest(IHttpRequest httpRequest)
	{
	    if (!serverRoutingTable.Routes.ContainsKey(httpRequest.RequestMethod)
		|| !serverRoutingTable.Routes[httpRequest.RequestMethod].ContainsKey(httpRequest.Path))
	    {
		return new HttpResponse(HttpResponseStatusCode.NotFound);
	    }
	    return serverRoutingTable.Routes[httpRequest.RequestMethod][httpRequest.Path].Invoke(httpRequest);
	}

	private void SetResponseSession(IHttpResponse httpResponse, IHttpCookie sessionCookie)
	{
	    httpResponse.Cookies.Add(sessionCookie);
	}

	private async Task PrepareResponse(IHttpResponse httpResponse)
	{
	    byte[] bytesToSend = httpResponse.GetBytes();
	    var dataBuffer = new ArraySegment<byte>(bytesToSend);
	    await client.SendAsync(dataBuffer, SocketFlags.None);
	}
    }
}
