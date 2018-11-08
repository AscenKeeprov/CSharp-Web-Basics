using SIS.HTTP.Requests.Contracts;
using SIS.HTTP.Responses.Contracts;

namespace IRunes.App.Controllers.Contracts
{
    public interface IAlbumsController
    {
	IHttpResponse BrowseAll(IHttpRequest request);
	IHttpResponse BrowseOne(IHttpRequest request);
	IHttpResponse CreateGet(IHttpRequest request);
	IHttpResponse CreatePost(IHttpRequest request);
    }
}
