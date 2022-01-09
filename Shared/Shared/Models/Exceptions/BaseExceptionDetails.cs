namespace Shared.Models.Exceptions
{
    public class BaseExceptionDetails
    {
        public BaseExceptionDetails()
        {
            Message = string.Empty;
        }

        public int StatusCode { get; set; }

        public string Message { get; set; }
    }
}
