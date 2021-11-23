using Shared.Configurations;
using System.Text.Json;

namespace Shared.Models
{
    public class RequestData
    {
        protected RequestData() { }

        public string BaseUrl { get; protected set; }

        public string Path { get; protected set; }

        public HttpMethod HttpMethod { get; protected set; }

        public string Content { get; protected set; }

        public string AccessToken { get; protected set; }

        public string AuthenticationScheme { get; protected set; }

        public static RequestData Get<T>(
            T content,
            string baseUrl,
            string path,
            HttpMethod httpMethod,
            string accessToken = "",
            string authenticationScheme = "Bearer")
        {
            var requestData = new RequestData
            {
                BaseUrl = baseUrl,
                Path = path,
                HttpMethod = httpMethod
            };

            if (content != null)
            {
                requestData.Content = JsonSerializer.Serialize(content, JsonOptionsConfiguration.Options);
            }

            if (!string.IsNullOrEmpty(accessToken) && !string.IsNullOrWhiteSpace(authenticationScheme))
            {
                requestData.AccessToken = accessToken;
                requestData.AuthenticationScheme = authenticationScheme;
            }

            return requestData;
        }

        public static RequestData Get(
            string baseUrl,
            string path,
            HttpMethod httpMethod,
            string accessToken = "",
            string authenticationScheme = "Bearer")
        {
            var requestData = new RequestData
            {
                BaseUrl = baseUrl,
                Path = path,
                HttpMethod = httpMethod
            };

            if (!string.IsNullOrEmpty(accessToken) && !string.IsNullOrWhiteSpace(authenticationScheme))
            {
                requestData.AccessToken = accessToken;
                requestData.AuthenticationScheme = authenticationScheme;
            }

            return requestData;
        }
    }
}
