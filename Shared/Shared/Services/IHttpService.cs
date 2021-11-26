using Shared.Models.OperationResults;
using Shared.Models.Requests;

namespace Shared.Services
{
    public interface IHttpService
    {
        Task<Result<TOutData>> Send<TOutData>(RequestData requestData);
    }
}
