using Shared.Configurations;
using Shared.Models.OperationResults;
using Shared.Models.Requests;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Shared.Services
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _httpClient;

        public HttpService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<Result<TOutData>> Send<TOutData>(RequestData requestData)
        {
            try
            {
                var response = await BaseSend(requestData);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode && !string.IsNullOrWhiteSpace(responseContent))
                {
                    var responseModel = JsonSerializer.Deserialize<Result<TOutData>>(responseContent, JsonOptionsConfiguration.Options);
                    return responseModel ?? Result.ServerError("Can not parse model");
                }

                return Result.ServerError(responseContent ?? "An error occurred on the request");
            }
            catch (Exception ex)
            {
                return Result.ServerError(ex.Message);
            }
        }

        private async Task<HttpResponseMessage> BaseSend(RequestData request)
        {
            var requestUri = new Uri($"{request.BaseUrl}{request.Path}");
            var message = new HttpRequestMessage(request.HttpMethod, requestUri);

            message.Headers.Add("Accept", "application/json");

            if (!string.IsNullOrWhiteSpace(request.Content))
            {
                message.Content = new StringContent(request.Content, Encoding.UTF8, "application/json");
            }

            _httpClient.DefaultRequestHeaders.Clear();

            if (!string.IsNullOrWhiteSpace(request.AccessToken)
                && !string.IsNullOrWhiteSpace(request.AuthenticationScheme))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(request.AuthenticationScheme, request.AccessToken);
            }

            var response = await _httpClient.SendAsync(message);
            return response;
        }
    }
}
