using System.Net;

namespace Shared.Models.ApiResponses
{
    public class ApiResponse<T> : ApiResponseBase
    {
        public T? Data { get; set; }

        public static implicit operator ApiResponse<T>(T value)
        {
            return new ApiResponse<T>
            {
                Data = value,
                StatusCode = (int)HttpStatusCode.OK,
            };
        }

        public static implicit operator ApiResponse<T>(ApiResponse response)
        {
            if (response == null)
                return new ApiResponse<T>();

            return new ApiResponse<T>
            {
                Data = default,
                StatusCode = response.StatusCode,
                ErrorMessage = response.ErrorMessage,
                ValidationMessages = response.ValidationMessages
            };
        }
    }
}
