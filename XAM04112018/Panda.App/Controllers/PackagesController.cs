using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Panda.App.Common;
using Panda.App.ViewModels;
using Panda.Models.Enumerations;
using Panda.Services.Contracts;
using SIS.Framework.ActionResults;
using SIS.Framework.Attributes.Method;

namespace Panda.App.Controllers
{
    public class PackagesController : BaseController
    {
	private readonly IUsersService usersService;
	private readonly IPackagesService packagesService;

	public PackagesController(IUsersService usersService, IPackagesService packagesService)
	{
	    this.usersService = usersService;
	    this.packagesService = packagesService;
	}

	[HttpGet]
	public IActionResult Create()
	{
	    Model["Recipients"] = GetRecipients();
	    return View();
	}

	[HttpPost]
	public IActionResult Create(PackageCreateViewModel model)
	{
	    if (ModelState.IsValid != true)
	    {
		return View();
	    }
	    if (packagesService.Exists(model.Description))
	    {
		Model["Error"] = $"A package with description '{model.Description}' already exists";
		Model["Recipients"] = GetRecipients();
		return View();
	    }
	    packagesService.AddPackage(model.Description, model.Weight, model.ShippingAddress, model.Recipient);
	    return RedirectToAction(Constants.HomeViewRoute);
	}

	[HttpGet]
	public IActionResult Deliver()
	{
	    int packageId = int.Parse(Request.QueryData["id"].ToString());
	    packagesService.DeliverPackage(packageId);
	    return RedirectToAction(Constants.HomeViewRoute);
	}

	[HttpGet]
	public IActionResult Details()
	{
	    int packageId = int.Parse(Request.QueryData["id"].ToString());
	    var package = packagesService.GetPackageById(packageId);
	    Model["ShippingAddress"] = package.ShippingAddress;
	    Model["Status"] = package.Status.ToString();
	    if (package.Status == Status.Pending)
	    {
		Model["EstimatedDeliveryDate"] = "N/A";
	    }
	    if (package.Status == Status.Shipped)
	    {
		Model["EstimatedDeliveryDate"] = package.EstimatedDeliveryDate
		    .Value.ToString("dd'/'MM'/'yyyy", CultureInfo.InvariantCulture);
	    }
	    if (package.Status == Status.Delivered || package.Status == Status.Acquired)
	    {
		Model["EstimatedDeliveryDate"] = "Delivered";
	    }
	    Model["Weight"] = $"{package.Weight:G3} KG";
	    Model["Recipient"] = package.Recipient.Username;
	    Model["Description"] = package.Description;
	    return View();
	}

	[HttpGet]
	public IActionResult Pending()
	{
	    var pendingPackages = packagesService.GetAllPackages()
		.Where(p => p.Status == Status.Pending)
		.ToArray();
	    var packagesTable = new List<PackagePendingViewModel>();
	    for (int i = 0; i < pendingPackages.Length; i++)
	    {
		var package = pendingPackages[i];
		var packageModel = new PackagePendingViewModel()
		{
		    Number = i + 1,
		    Id = package.Id,
		    Description = package.Description,
		    Weight = $"{package.Weight:G3} KG",
		    ShippingAddress = package.ShippingAddress,
		    Recipient = package.Recipient.Username
		};
		packagesTable.Add(packageModel);
	    }
	    Model["Packages"] = packagesTable;
	    return View();
	}

	[HttpGet]
	public IActionResult Ship()
	{
	    int packageId = int.Parse(Request.QueryData["id"].ToString());
	    packagesService.ShipPackage(packageId);
	    return RedirectToAction(Constants.HomeViewRoute);
	}

	private IEnumerable<RecipientViewModel> GetRecipients()
	{
	    return usersService.GetAllUsers()
		.Select(recipient => new RecipientViewModel()
		{
		    Name = recipient.Username
		});
	}

	[HttpGet]
	public IActionResult Shipped()
	{
	    var shippedPackages = packagesService.GetAllPackages()
		.Where(p => p.Status == Status.Shipped)
		.ToArray();
	    var packagesTable = new List<PackageShippedViewModel>();
	    for (int i = 0; i < shippedPackages.Length; i++)
	    {
		var package = shippedPackages[i];
		var packageModel = new PackageShippedViewModel()
		{
		    Number = i + 1,
		    Id = package.Id,
		    Description = package.Description,
		    Weight = $"{package.Weight:G3} KG",
		    EstimatedDeliveryDate = package.EstimatedDeliveryDate.Value.ToString("dd'/'MM'/'yyyy", CultureInfo.InvariantCulture),
		    Recipient = package.Recipient.Username
		};
		packagesTable.Add(packageModel);
	    }
	    Model["Packages"] = packagesTable;
	    return View();
	}
    }
}
