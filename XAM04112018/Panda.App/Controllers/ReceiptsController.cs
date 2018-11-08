using System.Globalization;
using System.Linq;
using Panda.App.ViewModels;
using Panda.Services.Contracts;
using SIS.Framework.ActionResults;
using SIS.Framework.Attributes.Method;

namespace Panda.App.Controllers
{
    public class ReceiptsController : BaseController
    {
	private readonly IReceiptsService receiptsService;

	public ReceiptsController(IReceiptsService receiptsService)
	{
	    this.receiptsService = receiptsService;
	}

	[HttpGet]
	public IActionResult Index()
	{
	    Model["Receipts"] = receiptsService.GetAllReceipts()
		.Select(r => new ReceiptViewModel()
		{
		    Id = r.Id,
		    Fee = $"${r.Fee:F2}",
		    IssueDate = r.IssuedOn.ToString("dd'/'MM'/'yyyy", CultureInfo.InvariantCulture),
		    Recipient = r.Recipient.Username
		});
	    return View();
	}
    }
}
