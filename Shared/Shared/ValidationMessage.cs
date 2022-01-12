namespace Shared.Models
{
    public class ValidationMessage
    {
        public string? Field { get; set; }

        public IList<string>? Messages { get; set; }
    }
}
