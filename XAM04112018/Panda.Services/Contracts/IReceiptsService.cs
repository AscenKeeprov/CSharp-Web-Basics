using System.Collections.Generic;
using Panda.Models;

namespace Panda.Services.Contracts
{
    public interface IReceiptsService
    {
	IEnumerable<Receipt> GetAllReceipts();
    }
}
