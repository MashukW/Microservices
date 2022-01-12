using Shared.Models.ApiResponses;
using Shared.Models.Requests;

namespace Shared.Services.Interfaces
{
    public interface IApiService
    {
        Task<ApiResponse<T>> Send<T>(RequestData requestData);
    }
}
