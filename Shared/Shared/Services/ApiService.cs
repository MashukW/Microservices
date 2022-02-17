using Microsoft.Extensions.Logging;
using Shared.Configurations;
using Shared.Models.ApiResponses;
using Shared.Models.Requests;
using Shared.Services.Interfaces;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Shared.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiService> _logger;

        public ApiService(IHttpClientFactory httpClientFactory, ILogger<ApiService> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
        }

        public async Task<ApiResponse<T>> Send<T>(ApiRequest requestData)
        {
            try
            {
                var response = await BaseSend(requestData);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(responseContent))
                {
                    var requestDataJson = JsonSerializer.Serialize(requestData, JsonOptionsConfiguration.Options);
                    _logger.LogCritical($"Response content is null or white space. Request details: {requestDataJson}.");

                    return new ApiResponse
                    {
                        StatusCode = (int)response.StatusCode,
                        ErrorMessage = "Failed request"
                    };
                }

                var apiResponse = JsonSerializer.Deserialize<ApiResponse<T>>(responseContent, JsonOptionsConfiguration.Options);
                if (apiResponse == null)
                {
                    var requestDataJson = JsonSerializer.Serialize(requestData, JsonOptionsConfiguration.Options);
                    _logger.LogCritical($"Can not parse response into the {typeof(T).FullName} model. Request details: {requestDataJson}. Response content: {responseContent}");

                    return new ApiResponse
                    {
                        StatusCode = (int)response.StatusCode,
                        ErrorMessage = "Failed request"
                    };
                }

                return apiResponse;
            }
            catch (Exception ex)
            {
                var requestDataJson = JsonSerializer.Serialize(requestData, JsonOptionsConfiguration.Options);
                _logger.LogCritical($"Api request failed. Request details: {requestDataJson}. Exception details: {ex.Message}");

                return new ApiResponse()
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    ErrorMessage = "Internal Server Error",
                };
            }
        }

        private async Task<HttpResponseMessage> BaseSend(ApiRequest request)
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
