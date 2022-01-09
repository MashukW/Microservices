using Shared.Models.OperationResults;

namespace Shared.Exceptions
{
    public class ValidationErrorException : Exception
    {
        public ValidationErrorException(ValidationMessage validationMessage)
            : base("Invalid request. Validation errors.")
        {
            ValidationMessages = new List<ValidationMessage>
            {
                validationMessage
            };
        }

        public ValidationErrorException(IList<ValidationMessage> validationMessages)
            : base("Invalid request. Validation errors.")
        {
            ValidationMessages = validationMessages;
        }

        public ValidationErrorException()
            : base("Invalid request. Validation errors.")
        {
            ValidationMessages = new List<ValidationMessage>();
        }

        public IList<ValidationMessage> ValidationMessages { get; private set; }
    }
}
