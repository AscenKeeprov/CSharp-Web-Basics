using System.Collections.Generic;

namespace Chushka.App.ViewModels
{
    public class GridViewModel
    {
	public GridViewModel()
	{
	    GridRows = new List<GridRowViewModel>();
	}

	public IEnumerable<GridRowViewModel> GridRows { get; set; }
    }
}
