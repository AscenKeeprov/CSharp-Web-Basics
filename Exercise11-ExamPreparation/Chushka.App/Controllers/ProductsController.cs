using System.Linq;
using Chushka.App.Common;
using Chushka.App.ViewModels;
using Chushka.Services.Contracts;
using SIS.Framework.ActionResults;
using SIS.Framework.Attributes.Method;

namespace Chushka.App.Controllers
{
    public class ProductsController : BaseController
    {
	private readonly IProductsService productsService;

	public ProductsController(IProductsService productsService)
	{
	    this.productsService = productsService;
	}

	[HttpGet]
	public IActionResult Create()
	{
	    Model["ProductTypes"] = GetProductTypes();
	    return View();
	}

	[HttpPost]
	public IActionResult Create(ProductCreateViewModel model)
	{
	    if (ModelState.IsValid != true)
	    {
		return View();
	    }
	    if (productsService.Exists(model.Name))
	    {
		Model["Error"] = $"Product '{model.Name}' is already in stock";
		Model["ProductTypes"] = GetProductTypes();
		return View();
	    }
	    productsService.AddProduct(model.Name, model.Price, model.Description, model.Type);
	    return RedirectToAction(Constants.HomeViewRoute);
	}

	[HttpGet]
	public IActionResult Delete()
	{
	    int productId = int.Parse(Request.QueryData["id"].ToString());
	    var product = productsService.GetProductById(productId);
	    Model["Id"] = product.Id;
	    Model["Name"] = product.Name;
	    Model["TypeId"] = (int)product.Type;
	    Model["Price"] = product.Price;
	    Model["Description"] = product.Description;
	    Model["ProductTypes"] = GetProductTypes();
	    return View();
	}

	[HttpPost]
	public IActionResult Delete(int id)
	{
	    productsService.Delete(id);
	    return RedirectToAction(Constants.HomeViewRoute);
	}

	[HttpGet]
	public IActionResult Details()
	{
	    int productId = int.Parse(Request.QueryData["id"].ToString());
	    var product = productsService.GetProductById(productId);
	    Model["Id"] = product.Id;
	    Model["Name"] = product.Name;
	    Model["Type"] = product.Type.ToString();
	    Model["Price"] = $"${product.Price:F2}";
	    Model["Description"] = product.Description;
	    return View();
	}

	[HttpGet]
	public IActionResult Edit()
	{
	    int productId = int.Parse(Request.QueryData["id"].ToString());
	    var product = productsService.GetProductById(productId);
	    Model["Id"] = product.Id;
	    Model["Name"] = product.Name;
	    Model["TypeId"] = (int)product.Type;
	    Model["Price"] = product.Price;
	    Model["Description"] = product.Description;
	    Model["ProductTypes"] = GetProductTypes();
	    return View();
	}

	[HttpPost]
	public IActionResult Edit(ProductCreateViewModel model)
	{
	    if (ModelState.IsValid != true)
	    {
		return View();
	    }
	    int productId = int.Parse(Request.QueryData["id"].ToString());
	    var existingProduct = productsService.GetProductByName(model.Name);
	    if (existingProduct != null && existingProduct.Id != productId)
	    {
		Model["Error"] = $"Product '{model.Name}' is already in stock";
		Model["Id"] = productId;
		Model["Name"] = model.Name;
		Model["Price"] = model.Price;
		Model["Description"] = model.Description;
		Model["TypeId"] = productsService.GetProductTypeId(model.Type);
		Model["ProductTypes"] = GetProductTypes();
		return View();
	    }
	    productsService.UpdateProduct(productId, model.Name, model.Price, model.Description, model.Type);
	    return RedirectToAction(Constants.HomeViewRoute);
	}

	private object GetProductTypes()
	{
	    return productsService.GetAllProductTypes()
		.Select(productType => new ProductTypeViewModel()
		{
		    ProductTypeId = (int)productType,
		    ProductTypeName = productType.ToString()
		});
	}
    }
}
