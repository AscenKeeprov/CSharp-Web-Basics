using SIS.Framework;

namespace Panda.App
{
    public class Launcher
    {
	public static void Main(string[] args)
	{
	    WebHost.Start(new Startup());
	}
    }
}
