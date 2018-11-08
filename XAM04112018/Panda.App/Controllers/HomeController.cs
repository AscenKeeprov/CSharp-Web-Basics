using System;
using System.Collections.Generic;
using System.Linq;
using Panda.App.ViewModels;
using Panda.Models.Enumerations;
using Panda.Services.Contracts;
using SIS.Framework.ActionResults;
using SIS.Framework.Attributes.Method;

namespace Panda.App.Controllers
{
    public class HomeController : BaseController
    {
	private readonly IUsersService usersService;
	private readonly IPackagesService packagesService;

	public HomeController(IUsersService usersService, IPackagesService packagesService)
	{
	    this.usersService = usersService;
	    this.packagesService = packagesService;
	}

	[HttpGet]
	public IActionResult Index()
	{
	    if (Identity != null)
	    {
		var user = usersService.GetUserByUsername(Identity.Username);
		Model["Username"] = user?.Username;
		Model["UserId"] = user?.Id;
		var packages = new List<PackageViewModel>();
		if (Identity.Roles.Contains(Role.Admin.ToString()))
		{
		    packages = packagesService.GetAllPackages()
			.Select(p => new PackageViewModel()
			{
			    Id = p.Id,
			    Description = p.Description,
			    Status = p.Status.ToString()
			}).ToList();
		    PopulatePackageLists(packages);
		    return View("Index-Admin");
		}
		if (Identity.Roles.Contains(Role.User.ToString()))
		{
		    packages = packagesService.GetAllPackagesForUser(user?.Username)
			.Select(p => new PackageViewModel()
			{
			    Id = p.Id,
			    Description = p.Description,
			    Status = p.Status.ToString()
			}).ToList();
		    PopulatePackageLists(packages);
		    return View("Index-User");
		}
	    }
	    return View();
	}

	private void PopulatePackageLists(IEnumerable<PackageViewModel> packages)
	{
	    Model["PendingPackages"] = packages
		.Where(p => p.Status == Status.Pending.ToString());
	    Model["ShippedPackages"] = packages
		.Where(p => p.Status == Status.Shipped.ToString());
	    Model["DeliveredPackages"] = packages
		.Where(p => p.Status == Status.Delivered.ToString());
	}
    }
}
