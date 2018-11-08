using System.Collections.Generic;

namespace Chushka.App.ViewModels
{
    public class ProductCreateViewModel
    {
	public string Name { get; set; }
	public decimal Price { get; set; }
	public string Description { get; set; }
	public string Type { get; set; }
	public IEnumerable<ProductTypeViewModel> ProductTypes { get; set; }
    }
}
