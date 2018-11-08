using System.Globalization;
using System.Linq;
using Chushka.App.Common;
using Chushka.App.ViewModels;
using Chushka.Services.Contracts;
using SIS.Framework.ActionResults;
using SIS.Framework.Attributes.Method;

namespace Chushka.App.Controllers
{
    public class OrdersController : BaseController
    {
	private readonly IOrdersService ordersService;

	public OrdersController(IOrdersService ordersService)
	{
	    this.ordersService = ordersService;
	}

	[HttpGet]
	public IActionResult All()
	{
	    Model["Orders"] = ordersService.GetAllOrders()
		.Select(order => new OrderViewModel()
		{
		    OrderId = order.Id,
		    CustomerName = order.Client.FullName ?? order.Client.Username,
		    ProductName = order.Product.Name,
		    OrderDate = order.OrderedOn.ToString("HH':'mm dd'/'MM'/'yyyy", CultureInfo.InvariantCulture)
		});
	    return View();
	}

	[HttpGet]
	public IActionResult Create()
	{
	    int productId = int.Parse(Request.QueryData["productId"].ToString());
	    string clientName = Identity.Username;
	    ordersService.CreateOrder(productId, clientName);
	    return RedirectToAction(Constants.HomeViewRoute);
	}
    }
}
