using System.Net;

namespace Shared.Web.Models.Responses
{
    public class Response<T> : BaseResponse
    {
        private Response()
        {

        }

        public T? Data { get; protected set; }

        public static implicit operator Response<T>(T? data)
        {
            return new Response<T>
            {
                Data = data,
                StatusCode = (int)HttpStatusCode.OK,
            };
        }

        public static implicit operator Response<T>(EmptyDataResponse response)
        {
            return new Response<T>
            {
                ErrorMessage = response.ErrorMessage,
                ValidationErrors = response.ValidationErrors,
                StatusCode = response.StatusCode,
            };
        }

        public static Response<T> Custom(T data, int statusCode)
        {
            return Custom(data, statusCode, default, default);
        }

        public static Response<T> Custom(T data, int statusCode, string? errorMessage)
        {
            return Custom(data, statusCode, errorMessage, default);
        }

        public static Response<T> Custom(T data, int statusCode, string? errorMessage, List<ValidationError>? validationErrors)
        {
            return new Response<T>
            {
                Data = data,
                StatusCode = statusCode,
                ErrorMessage = errorMessage,
                ValidationErrors = validationErrors
            };
        }
    }

    public class EmptyDataResponse : BaseResponse
    {
        protected EmptyDataResponse()
        {

        }

        public static EmptyDataResponse Success()
        {
            return new EmptyDataResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
            };
        }

        public static EmptyDataResponse Fail(string? errorMessage)
        {
            return new EmptyDataResponse
            {
                ErrorMessage = errorMessage,
                StatusCode = (int)HttpStatusCode.InternalServerError,
            };
        }

        public static EmptyDataResponse ValidationFail(IList<ValidationError>? validationErrors)
        {
            return new EmptyDataResponse
            {
                ValidationErrors = validationErrors,
                StatusCode = (int)HttpStatusCode.BadRequest,
            };
        }

        public static EmptyDataResponse Custom(int statusCode)
        {
            return Custom(statusCode, default, default);
        }

        public static EmptyDataResponse Custom(int statusCode, string? errorMessage)
        {
            return Custom(statusCode, errorMessage, default);
        }

        public static EmptyDataResponse Custom(int statusCode, string? errorMessage, List<ValidationError>? validationErrors)
        {
            return new EmptyDataResponse
            {
                StatusCode = statusCode,
                ErrorMessage = errorMessage,
                ValidationErrors = validationErrors
            };
        }
    }

    public class BaseResponse
    {
        public bool IsSuccess => StatusCode == 200;

        public int StatusCode { get; protected set; }

        public string? ErrorMessage { get; protected set; }

        public IList<ValidationError>? ValidationErrors { get; protected set; }
    }
}
