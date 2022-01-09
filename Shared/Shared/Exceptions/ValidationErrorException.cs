using Shared.Models.OperationResults;

namespace Shared.Exceptions
{
    public class ValidationErrorException : Exception
    {
        public ValidationErrorException()
            : base("Invalid request. Validation errors.")
        {
            ValidationMessages = new List<ValidationMessage>();
        }

        public IList<ValidationMessage> ValidationMessages { get; set; }
    }
}
