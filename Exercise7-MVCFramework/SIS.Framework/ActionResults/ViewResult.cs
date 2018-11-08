using SIS.Framework.ActionResults.Contracts;

namespace SIS.Framework.ActionResults
{
    public class ViewResult : IViewable
    {
	//TODO: OR IRenderer renderer ???
	public ViewResult(IRenderable view)
	{
	    View = view;
	}

	//TODO: OR IRenderer Renderer ???
	public IRenderable View { get; set; }

	//TODO: OR View() => Renderer.Render() ???
	public string Invoke() => View.Render();
    }
}
