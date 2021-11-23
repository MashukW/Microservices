using Shared.Web.Extensions;
using Shared.Web.Models;
using Shared.Web.Models.Responses;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Shared.Web.Services
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;

        public HttpService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<Response<T>> Send<T>(RequestDetails requestOptions)
        {
            try
            {
                var response = await BaseSend(requestOptions);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var responseModel = JsonSerializer.Deserialize<T>(responseContent, JsonOptionsConfiguration.Options);
                    return responseModel;
                }

                return EmptyDataResponse.Custom((int)response.StatusCode, responseContent);
            }
            catch (Exception ex)
            {
                return EmptyDataResponse.Fail(ex.Message);
            }
        }

        public async Task<EmptyDataResponse> Send(RequestDetails requestOptions)
        {
            try
            {
                var response = await BaseSend(requestOptions);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return EmptyDataResponse.Success();
                }

                return EmptyDataResponse.Custom((int)response.StatusCode, responseContent);
            }
            catch (Exception ex)
            {
                return EmptyDataResponse.Fail(ex.Message);
            }
        }

        private async Task<HttpResponseMessage> BaseSend(RequestDetails requestOptions)
        {
            var requestUri = new Uri($"{requestOptions.BaseUrl}{requestOptions.Path}");
            var message = new HttpRequestMessage(requestOptions.HttpMethod, requestUri);

            message.Headers.Add("Accept", "application/json");

            _httpClient.DefaultRequestHeaders.Clear();

            if (requestOptions.Content != default)
            {
                var content = JsonSerializer.Serialize(requestOptions.Content, JsonOptionsConfiguration.Options);
                message.Content = new StringContent(content, Encoding.UTF8, "application/json");
            }

            if (!string.IsNullOrEmpty(requestOptions.AccessToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(requestOptions.AuthenticationScheme, requestOptions.AccessToken);
            }

            var response = await _httpClient.SendAsync(message);
            return response;
        }
    }
}
