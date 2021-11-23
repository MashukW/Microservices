using System.Net;

namespace Shared.Models.Responses
{
    public class ResponseData<T>
    {
        private ResponseData()
        {

        }

        public bool IsSuccess => StatusCode == 200;

        public int StatusCode { get; protected set; }

        public T? Data { get; protected set; }

        public string? ErrorMessage { get; protected set; }

        public IList<ValidationError>? ValidationErrors { get; protected set; }

        public static implicit operator ResponseData<T>(T? data)
        {
            return new ResponseData<T>
            {
                Data = data,
                StatusCode = (int)HttpStatusCode.OK,
            };
        }

        public static ResponseData<T> Custom(T? data, int statusCode)
        {
            return Custom(data, statusCode, default, default);
        }

        public static ResponseData<T> Custom(T? data, int statusCode, string? errorMessage)
        {
            return Custom(data, statusCode, errorMessage, default);
        }

        public static ResponseData<T> Custom(T? data, int statusCode, string? errorMessage, List<ValidationError>? validationErrors)
        {
            return new ResponseData<T>
            {
                Data = data,
                StatusCode = statusCode,
                ErrorMessage = errorMessage,
                ValidationErrors = validationErrors
            };
        }

        public static ResponseData<T> Fail(string? errorMessage)
        {
            return new ResponseData<T>
            {
                ErrorMessage = errorMessage,
                StatusCode = (int)HttpStatusCode.InternalServerError,
            };
        }

        public static ResponseData<T> ValidationFail(IList<ValidationError>? validationErrors)
        {
            return new ResponseData<T>
            {
                ValidationErrors = validationErrors,
                StatusCode = (int)HttpStatusCode.BadRequest,
            };
        }
    }
}