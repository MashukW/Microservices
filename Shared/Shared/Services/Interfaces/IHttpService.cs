using Shared.Models.OperationResults;
using Shared.Models.Requests;

namespace Shared.Services.Interfaces
{
    public interface IHttpService
    {
        Task<Result<TOutData>> Send<TOutData>(RequestData requestData);
    }
}
