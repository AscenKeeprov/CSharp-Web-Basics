using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using SIS.WebServer.Contracts;
using SIS.WebServer.Routing;

namespace SIS.WebServer
{
    public class Server : IServer
    {
	private const string LocalhostIpAddress = "127.0.0.1";
	private readonly int port;
	private readonly TcpListener listener;
	private readonly ServerRoutingTable serverRoutingTable;
	private bool isRunning;

	public Server(int port, ServerRoutingTable serverRoutingTable)
	{
	    this.port = port;
	    this.serverRoutingTable = serverRoutingTable;
	    listener = new TcpListener(IPAddress.Parse(LocalhostIpAddress), port);
	}

	public void Run()
	{
	    listener.Start();
	    if (listener.Server.IsBound) isRunning = true;
	    Console.WriteLine($"Server started at http://{LocalhostIpAddress}:{port}");
	    var task = Task.Run(ListeLoop);
	    task.Wait();
	}

	public async Task ListeLoop()
	{
	    while (isRunning)
	    {
		Socket client = await listener.AcceptSocketAsync();
		var connectionHandler = new ConnectionHandler(client, serverRoutingTable);
		var responseTask = connectionHandler.ProcessRequestAsync();
		responseTask.Wait();
	    }
	}
    }
}
