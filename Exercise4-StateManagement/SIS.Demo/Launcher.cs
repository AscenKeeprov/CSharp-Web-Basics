using SIS.HTTP.Enums;
using SIS.WebServer;
using SIS.WebServer.Routing;

namespace SIS.Demo
{
    public class Launcher
    {
	public static void Main()
	{
	    ServerRoutingTable serverRoutingTable = new ServerRoutingTable();
	    serverRoutingTable.AddRoute(HttpRequestMethod.Get, "/", new HomeController().Index());
	    Server server = new Server(8000, serverRoutingTable);
	    server.Run();
	}
    }
}
