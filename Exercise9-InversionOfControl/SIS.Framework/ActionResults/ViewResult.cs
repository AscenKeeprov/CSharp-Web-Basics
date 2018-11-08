using SIS.Framework.ActionResults.Contracts;

namespace SIS.Framework.ActionResults
{
    public class ViewResult : IViewable
    {
	public ViewResult(IRenderable view)
	{
	    View = view;
	}

	public IRenderable View { get; set; }

	public string Invoke() => View.Render();
    }
}
