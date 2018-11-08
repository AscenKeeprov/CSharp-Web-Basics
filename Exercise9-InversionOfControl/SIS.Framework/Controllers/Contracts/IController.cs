using SIS.Framework.Models;
using SIS.HTTP.Requests.Contracts;

namespace SIS.Framework.Controllers.Contracts
{
    public interface IController
    {
	Model ModelState { get; }
	string Name { get; }
	IHttpRequest Request { get; set; }
    }
}
