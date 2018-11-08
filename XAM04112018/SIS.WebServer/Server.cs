using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SIS.WebServer.Api;

namespace SIS.WebServer
{
    public class Server
    {
	private const string LocalhostIpAddress = "127.0.0.1";

	private readonly int port;

	private readonly TcpListener listener;

	private readonly IHttpRequestHandler httpRequestHandler;

	private bool isRunning;

	private Server(int port)
	{
	    this.port = port;
	    this.listener = new TcpListener(IPAddress.Parse(LocalhostIpAddress), this.port);
	}

	public Server(int port, IHttpRequestHandler httpRequestHandler) : this(port)
	{
	    this.httpRequestHandler = httpRequestHandler;
	}

	public void Run()
	{
	    this.listener.Start();
	    this.isRunning = true;

	    Console.WriteLine($"Server started at http://{LocalhostIpAddress}:{this.port}");
	    TryOpenBrowser($"http://{LocalhostIpAddress}:{this.port}");
	    while (isRunning)
	    {
		Console.WriteLine("Waiting for client...");

		var client = listener.AcceptSocketAsync().GetAwaiter().GetResult();

		Task.Run(() => Listen(client));
	    }
	}

	private void TryOpenBrowser(string url)
	{
	    try
	    {
		var processInfo = new ProcessStartInfo();
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
		    processInfo.FileName = "cmd.exe";
		    processInfo.Arguments = $"/C START {url.Replace("&", "^&")} /B";
		}
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
		{
		    processInfo.FileName = "xdg-open";
		    processInfo.Arguments = url;
		}

		if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
		{
		    processInfo.FileName = "open";
		    processInfo.Arguments = url;
		}
		Process.Start(processInfo);
	    }
	    catch { return; }
	}

	public async void Listen(Socket client)
	{
	    ConnectionHandler connectionHandler = new ConnectionHandler(client, this.httpRequestHandler);
	    await connectionHandler.ProcessRequestAsync();
	}
    }
}
