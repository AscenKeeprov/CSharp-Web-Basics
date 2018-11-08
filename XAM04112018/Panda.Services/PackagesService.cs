using System;
using System.Collections.Generic;
using System.Linq;
using Panda.Data;
using Panda.Models;
using Panda.Models.Enumerations;
using Panda.Services.Contracts;

namespace Panda.Services
{
    public class PackagesService : IPackagesService
    {
	private readonly PandaDbContext context;

	public PackagesService(PandaDbContext context)
	{
	    this.context = context;
	}

	public void AddPackage(string description, double weight, string shippingAddress, string recipientName)
	{
	    var recipient = context.Users.SingleOrDefault(u => u.Username == recipientName);
	    Package package = new Package()
	    {
		Description = description,
		Weight = weight,
		ShippingAddress = shippingAddress,
		Recipient = recipient
	    };
	    context.Packages.Add(package);
	    context.SaveChanges();
	}

	public void DeliverPackage(int id)
	{
	    Package package = context.Packages.Find(id);
	    package.Status = Status.Delivered;
	    context.SaveChanges();
	}

	public bool Exists(string description)
	{
	    return context.Packages.Any(p => p.Description == description);
	}

	public IEnumerable<Package> GetAllPackages()
	{
	    return context.Packages.AsEnumerable();
	}

	public IEnumerable<Package> GetAllPackagesForUser(string username)
	{
	    return context.Packages
		.Where(p => p.Recipient.Username == username)
		.AsEnumerable();
	}

	public Package GetPackageById(int id)
	{
	    return context.Packages.Find(id);
	}

	public void ShipPackage(int id)
	{
	    Package package = context.Packages.Find(id);
	    package.Status = Status.Shipped;
	    Random rng = new Random();
	    package.EstimatedDeliveryDate = DateTime.UtcNow.AddDays(rng.Next(20, 41));
	    context.SaveChanges();
	}
    }
}
