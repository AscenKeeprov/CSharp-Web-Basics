using System.Collections.Generic;
using Chushka.Models.Enumerations;

namespace Chushka.Models
{
    public class Product
    {
	public Product()
	{
	    Orders = new HashSet<Order>();
	}

	public int Id { get; set; }
	public string Name { get; set; }
	public decimal Price { get; set; }
	public string Description { get; set; }
	public ProductType Type { get; set; }
	public bool IsDeleted { get; set; }
	public virtual IEnumerable<Order> Orders { get; set; }
    }
}
