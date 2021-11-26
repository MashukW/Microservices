namespace Shared.Models.OperationResults
{
    public class FailureInfo
    {
        public FailureType FailureType { get; set; }

        public string? Message { get; set; }

        public IList<ValidationMessage>? ValidationMessages { get; set; }
    }
}
