using Chushka.Data;
using Chushka.Services;
using Chushka.Services.Contracts;
using SIS.Framework.Api;
using SIS.Framework.Services;

namespace Chushka.App
{
    public class Startup : MvcApplication
    {
	public Startup()
	{
	    using (var dbContext = new ChushkaDbContext())
	    {
		dbContext.Database.EnsureCreated();
	    }
	}

	public override void Configure() { }

	public override void ConfigureServices(IDependencyContainer dependencies)
	{
	    dependencies.RegisterDependency<ChushkaDbContext, ChushkaDbContext>();
	    dependencies.RegisterDependency<IUsersService, UsersService>();
	    dependencies.RegisterDependency<IProductsService, ProductsService>();
	    dependencies.RegisterDependency<IOrdersService, OrdersService>();
	    base.ConfigureServices(dependencies);
	}
    }
}
