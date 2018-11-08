using System.Collections.Generic;
using Panda.Models;

namespace Panda.Services.Contracts
{
    public interface IPackagesService
    {
	void AddPackage(string description, double weight, string shippingAddress, string recipientName);
	bool Exists(string description);
	void DeliverPackage(int id);
	IEnumerable<Package> GetAllPackages();
	IEnumerable<Package> GetAllPackagesForUser(string username);
	Package GetPackageById(int id);
	void ShipPackage(int id);
    }
}
