using SIS.Framework;

namespace Chushka.App
{
    public class Launcher
    {
	public static void Main(string[] args)
	{
	    WebHost.Start(new Startup());
	}
    }
}
