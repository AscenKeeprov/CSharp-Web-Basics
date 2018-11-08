using SIS.Framework.ActionResults.Contracts;

namespace SIS.Framework.ActionResults
{
    public class UnauthorizedResult : IUnauthorized
    {
	public UnauthorizedResult(IRenderable view)
	{
	    View = view;
	}

	public IRenderable View { get; set; }

	public string Invoke() => View.Render();
    }
}
