using Panda.Data;
using Panda.Services;
using Panda.Services.Contracts;
using SIS.Framework.Api;
using SIS.Framework.Services;

namespace Panda.App
{
    public class Startup : MvcApplication
    {
	public Startup()
	{
	    using (var dbContext = new PandaDbContext())
	    {
		dbContext.Database.EnsureCreated();
	    }
	}

	public override void Configure() { }

	public override void ConfigureServices(IDependencyContainer dependencies)
	{
	    dependencies.RegisterDependency<PandaDbContext, PandaDbContext>();
	    dependencies.RegisterDependency<IUsersService, UsersService>();
	    dependencies.RegisterDependency<IPackagesService, PackagesService>();
	    dependencies.RegisterDependency<IReceiptsService, ReceiptsService>();
	    base.ConfigureServices(dependencies);
	}
    }
}
