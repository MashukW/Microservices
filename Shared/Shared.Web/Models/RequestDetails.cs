namespace Shared.Web.Models
{
    public class RequestDetails
    {
        public RequestDetails(string baseUrl, string path, HttpMethod httpMethod)
        {
            BaseUrl = baseUrl;
            Path = path;
            HttpMethod = httpMethod;
        }

        public string BaseUrl { get; private set; }

        public string Path { get; private set; }

        public HttpMethod HttpMethod { get; private set; }

        public object Content { get; set; }

        public string AccessToken { get; set; }

        public string AuthenticationScheme { get; set; } = "Bearer";
    }
}
