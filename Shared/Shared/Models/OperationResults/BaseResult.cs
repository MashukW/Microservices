namespace Shared.Models.OperationResults
{
    public abstract class BaseResult
    {
        public bool IsSuccess { get; set; }

        public FailureInfo? Failure { get; set; }
    }
}
