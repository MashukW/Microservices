using System.Net;

namespace Shared.Api.Outcomes
{
    public class ApiOutcomeData<T> : ApiOutcomeBase
    {
        public T? Data { get; set; }

        public static implicit operator ApiOutcomeData<T>(T value)
        {
            return new ApiOutcomeData<T>
            {
                Data = value,
                StatusCode = (int)HttpStatusCode.OK,
            };
        }

        public static implicit operator ApiOutcomeData<T>(ApiOutcome outcome)
        {
            if (outcome == null)
                return new ApiOutcomeData<T>();

            return new ApiOutcomeData<T>
            {
                Data = default,
                StatusCode = outcome.StatusCode,
                ErrorMessage = outcome.ErrorMessage,
                ValidationMessages = outcome.ValidationMessages
            };
        }
    }
}
