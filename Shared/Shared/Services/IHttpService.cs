using Shared.Models;
using Shared.Models.Responses;

namespace Shared.Services
{
    public interface IHttpService
    {
        Task<ResponseData<TOutData>> Send<TOutData>(RequestData requestData);
    }
}
