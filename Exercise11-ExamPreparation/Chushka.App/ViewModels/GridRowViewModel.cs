using System.Collections.Generic;

namespace Chushka.App.ViewModels
{
    public class GridRowViewModel
    {
	public GridRowViewModel()
	{
	    GridColumns = new List<GridColumnViewModel>();
	}

	public IEnumerable<GridColumnViewModel> GridColumns { get; set; }
    }
}
