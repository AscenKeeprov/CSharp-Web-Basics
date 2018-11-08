using Microsoft.EntityFrameworkCore;
using Panda.Data.EntityConfiguration;
using Panda.Models;

namespace Panda.Data
{
    public class PandaDbContext : DbContext
    {
	public virtual DbSet<User> Users { get; set; }
	public virtual DbSet<Package> Packages { get; set; }
	public virtual DbSet<Receipt> Receipts { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
	    optionsBuilder
		.UseSqlServer("Server=.\\SQLEXPRESS;Database=PandaDB;Integrated Security=True;")
		.UseLazyLoadingProxies(true);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
	    modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
	    modelBuilder.ApplyConfiguration(new PackageEntityConfiguration());
	    modelBuilder.ApplyConfiguration(new ReceiptEntityConfiguration());
	}
    }
}
