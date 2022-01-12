using Shared.Models;

namespace Shared.Api.Outcomes
{
    public abstract class ApiOutcomeBase
    {
        public bool IsSuccess => StatusCode == 200;

        public int StatusCode { get; set; }

        public string? ErrorMessage { get; set; }

        public IList<ValidationMessage>? ValidationMessages { get; set; }
    }
}
