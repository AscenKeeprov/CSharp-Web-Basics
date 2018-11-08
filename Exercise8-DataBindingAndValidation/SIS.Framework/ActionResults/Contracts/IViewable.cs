namespace SIS.Framework.ActionResults.Contracts
{
    public interface IViewable : IActionResult
    {
	//TODO: OR IRenderer Renderer ???
	IRenderable View { get; set; }
    }
}
