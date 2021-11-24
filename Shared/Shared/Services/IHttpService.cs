using Shared.Models.Requests;
using Shared.Models.Responses;

namespace Shared.Services
{
    public interface IHttpService
    {
        Task<ResponseData<TOutData>> Send<TOutData>(RequestData requestData);
    }
}
