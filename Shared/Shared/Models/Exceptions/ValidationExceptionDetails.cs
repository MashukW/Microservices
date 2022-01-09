using Shared.Models.OperationResults;

namespace Shared.Models.Exceptions
{
    public class ValidationExceptionDetails : BaseExceptionDetails
    {
        public ValidationExceptionDetails()
        {
            ValidationMessages = new List<ValidationMessage>();
        }

        public IList<ValidationMessage> ValidationMessages { get; set; }
    }
}
