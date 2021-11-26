namespace Shared.Models.OperationResults
{
    public enum FailureType
    {
        ServerError = 1,
        ValidationError = 2,
        NotFound = 3,
        Unauthorized = 4,
        Forbidden = 5
    }
}
