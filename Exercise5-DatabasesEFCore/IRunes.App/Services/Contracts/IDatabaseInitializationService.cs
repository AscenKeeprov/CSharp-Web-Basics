using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace IRunes.App.Services.Contracts
{
    public interface IDatabaseInitializationService
    {
	Task<bool> InitializeDatabaseAsync(DbContext dbContext);
    }
}
