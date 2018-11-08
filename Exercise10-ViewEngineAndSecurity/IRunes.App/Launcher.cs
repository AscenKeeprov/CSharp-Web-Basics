using SIS.Framework;

namespace IRunes.App
{
    public class Launcher
    {
	public static void Main()
	{
	    WebHost.Start(new Startup());
	}
    }
}
