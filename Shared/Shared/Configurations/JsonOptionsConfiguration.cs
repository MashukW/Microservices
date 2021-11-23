using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shared.Configurations
{
    public static class JsonOptionsConfiguration
    {
        private static JsonSerializerOptions? _options;

        public static JsonSerializerOptions Options
        {
            get
            {
                if (_options == null)
                {
                    _options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        // DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    };

                    _options.Converters.Add(new JsonStringEnumConverter());
                }

                return _options;
            }
        }
    }
}
