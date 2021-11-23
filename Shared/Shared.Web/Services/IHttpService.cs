using Shared.Web.Models;
using Shared.Web.Models.Responses;

namespace Shared.Web.Services
{
    public interface IHttpService
    {
        Task<EmptyDataResponse> Send(RequestDetails requestOptions);

        Task<Response<T>> Send<T>(RequestDetails requestOptions);
    }
}
