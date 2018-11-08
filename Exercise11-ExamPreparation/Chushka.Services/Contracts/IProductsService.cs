using System.Collections.Generic;
using Chushka.Models;
using Chushka.Models.Enumerations;

namespace Chushka.Services.Contracts
{
    public interface IProductsService
    {
	IEnumerable<Product> GetAllProducts();
	IEnumerable<ProductType> GetAllProductTypes();
	bool Exists(string name);
	void AddProduct(string name, decimal price, string description, string type);
	Product GetProductById(int id);
	Product GetProductByName(string name);
	void UpdateProduct(int id, string name, decimal price, string description, string type);
	int GetProductTypeId(string typeName);
	void Delete(int id);
    }
}
