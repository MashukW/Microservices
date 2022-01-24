using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Configurations;
using Shared.Exceptions;
using Shared.Models.ApiResponses;
using System.Net;
using System.Text.Json;

namespace Shared.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationErrorException validationErrorException)
            {
                await HandleExceptionAsync(context, new ApiResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    ErrorMessage = validationErrorException.Message,
                    ValidationMessages = validationErrorException.ValidationMessages
                });
            }
            catch (NotFoundException notFoundException)
            {
                await HandleExceptionAsync(context, new ApiResponse
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    ErrorMessage = notFoundException.Message,
                });
            }
            catch (UnauthorizedException unauthorizedException)
            {
                await HandleExceptionAsync(context, new ApiResponse
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                    ErrorMessage = unauthorizedException.Message,
                });
            }
            catch (ForbiddenException forbiddenException)
            {
                await HandleExceptionAsync(context, new ApiResponse
                {
                    StatusCode = (int)HttpStatusCode.Forbidden,
                    ErrorMessage = forbiddenException.Message,
                });
            }
            catch (ErrorException errorException)
            {
                await HandleExceptionAsync(context, new ApiResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    ErrorMessage = errorException.Message,
                });
            }
            catch
            {
                await HandleExceptionAsync(context, new ApiResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    ErrorMessage = "Internal Server Error",
                });
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, ApiResponse response)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = response.StatusCode;

            var responseJson = JsonSerializer.Serialize(response, JsonOptionsConfiguration.Options);

            await context.Response.WriteAsync(responseJson);
        }
    }
}
