using Chushka.Data.EntityConfiguration;
using Chushka.Models;
using Microsoft.EntityFrameworkCore;

namespace Chushka.Data
{
    public class ChushkaDbContext : DbContext
    {
	public virtual DbSet<User> Users { get; set; }
	public virtual DbSet<Product> Products { get; set; }
	public virtual DbSet<Order> Orders { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
	    optionsBuilder
		.UseSqlServer("Server=.\\SQLEXPRESS;Database=ChushkaDB;Integrated Security=True;")
		.UseLazyLoadingProxies(true);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
	    modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
	    modelBuilder.ApplyConfiguration(new ProductEntityConfiguration());
	    modelBuilder.ApplyConfiguration(new OrderEntityConfiguration());
	}
    }
}
