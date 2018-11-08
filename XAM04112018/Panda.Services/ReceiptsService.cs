using System.Collections.Generic;
using System.Linq;
using Panda.Data;
using Panda.Models;
using Panda.Services.Contracts;

namespace Panda.Services
{
    public class ReceiptsService : IReceiptsService
    {
	private readonly PandaDbContext context;

	public ReceiptsService(PandaDbContext context)
	{
	    this.context = context;
	}

	public IEnumerable<Receipt> GetAllReceipts()
	{
	    return context.Receipts.AsEnumerable();
	}
    }
}
