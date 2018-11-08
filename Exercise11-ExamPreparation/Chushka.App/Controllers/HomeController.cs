using System.Linq;
using Chushka.App.ViewModels;
using Chushka.Models.Enumerations;
using Chushka.Services.Contracts;
using SIS.Framework.ActionResults;

namespace Chushka.App.Controllers
{
    public class HomeController : BaseController
    {
	private const int GridColumnsCount = 5;
	private readonly IProductsService productsService;

	public HomeController(IProductsService productsService)
	{
	    this.productsService = productsService;
	}

	public IActionResult Index()
	{
	    if (Identity != null)
	    {
		Model["Username"] = Identity.Username;
		var products = productsService.GetAllProducts().ToList();
		var rowsCount = products.Count / GridColumnsCount + 1;
		var gridRows = new GridRowViewModel[rowsCount];
		for (int p = 0; p < products.Count; p++)
		{
		    for (int r = 0; r <= rowsCount; r++)
		    {
			var gridColumns = new GridColumnViewModel[GridColumnsCount];
			for (int c = 0; c < gridColumns.Length; c++)
			{
			    var column = new GridColumnViewModel()
			    {
				GridColumn = new ProductViewModel()
				{
				    Id = products[p].Id,
				    Name = products[p].Name,
				    Description = products[p].Description.Length > 53
					? $"{string.Join("", products[p].Description.Take(50))}..."
					: products[p].Description,
				    Price = $"${products[p].Price:F2}"
				}
			    };
			    gridColumns[c] = column;
			    p++;
			    if (p >= products.Count) break;
			}
			var row = new GridRowViewModel()
			{
			    GridColumns = gridColumns.Where(gc => gc != null).ToArray()
			};
			gridRows[r] = row;
			if (p >= products.Count) break;
		    }
		}
		Model["ProductsGrid"] = new GridViewModel()
		{
		    GridRows = gridRows
		};
		if (Identity.Roles.Contains(UserRole.Admin.ToString()))
		{
		    return View("Index-Admin");
		}
		if (Identity.Roles.Contains(UserRole.User.ToString()))
		{
		    return View("Index-User");
		}
	    }
	    return View();
	}
    }
}
