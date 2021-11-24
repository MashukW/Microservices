namespace Shared.Models
{
    public class OperationResult<T>
    {
        public bool IsSuccess { get; set; }

        public T? Data { get; set; }

        public string? ErrorMessage { get; set; }

        public IList<ValidationError>? ValidationErrors { get; set; }

        public static implicit operator OperationResult<T>(T? data)
        {
            return new OperationResult<T>
            {
                Data = data,
                IsSuccess = true
            };
        }

        public static OperationResult<T> Fail(string? errorMessage)
        {
            return new OperationResult<T>
            {
                ErrorMessage = errorMessage,
                IsSuccess = false
            };
        }

        public static OperationResult<T> ValidationFail(IList<ValidationError>? validationErrors)
        {
            return new OperationResult<T>
            {
                ValidationErrors = validationErrors,
                IsSuccess = false
            };
        }
    }
}
