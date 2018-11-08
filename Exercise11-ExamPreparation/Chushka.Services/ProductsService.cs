using System;
using System.Collections.Generic;
using System.Linq;
using Chushka.Data;
using Chushka.Models;
using Chushka.Models.Enumerations;
using Chushka.Services.Contracts;

namespace Chushka.Services
{
    public class ProductsService : IProductsService
    {
	private readonly ChushkaDbContext context;

	public ProductsService(ChushkaDbContext context)
	{
	    this.context = context;
	}

	public void AddProduct(string name, decimal price, string description, string type)
	{
	    Product product = new Product()
	    {
		Name = name,
		Description = description,
		Price = price,
		Type = Enum.Parse<ProductType>(type)
	    };
	    context.Products.Add(product);
	    context.SaveChanges();
	}

	public void Delete(int id)
	{
	    Product product = context.Products.Find(id);
	    context.Products.Remove(product);
	    context.SaveChanges();
	}

	public bool Exists(string name)
	{
	    return context.Products.Any(p => p.Name == name);
	}

	public IEnumerable<Product> GetAllProducts()
	{
	    return context.Products
		.Where(p => !p.IsDeleted)
		.AsEnumerable();
	}

	public IEnumerable<ProductType> GetAllProductTypes()
	{
	    return Enum.GetNames(typeof(ProductType))
		.Select(name => Enum.Parse<ProductType>(name));
	}

	public Product GetProductById(int id)
	{
	    return context.Products.Find(id);
	}

	public Product GetProductByName(string name)
	{
	    return context.Products.SingleOrDefault(p => p.Name == name);
	}

	public int GetProductTypeId(string typeName)
	{
	    return (int)Enum.Parse<ProductType>(typeName);
	}

	public void UpdateProduct(int id, string name, decimal price, string description, string type)
	{
	    Product product = context.Products.Find(id);
	    product.Name = name;
	    product.Price = price;
	    product.Description = description;
	    product.Type = Enum.Parse<ProductType>(type);
	    context.SaveChanges();
	}
    }
}
