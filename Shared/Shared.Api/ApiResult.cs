using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Shared.Models.OperationResults;
using System.Net;

namespace Shared.Api
{
    public class ApiResult : ActionResult, IStatusCodeActionResult, IActionResult
    {
        public Result? Value { get; protected set; }

        public int? StatusCode { get; protected set; }

        public static implicit operator ApiResult(Result value)
        {
            if (value == null)
            {
                return new ApiResult
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                };
            }

            var statusCode = value switch
            {
                { IsSuccess: true } => (int)HttpStatusCode.OK,
                { IsSuccess: false } and { Failure: not null } and { Failure: { FailureType: FailureType.ServerError } } => (int)HttpStatusCode.InternalServerError,
                { IsSuccess: false } and { Failure: not null } and { Failure: { FailureType: FailureType.ValidationError } } => (int)HttpStatusCode.BadRequest,
                { IsSuccess: false } and { Failure: not null } and { Failure: { FailureType: FailureType.NotFound } } => (int)HttpStatusCode.NotFound,
                { IsSuccess: false } and { Failure: not null } and { Failure: { FailureType: FailureType.Unauthorized } } => (int)HttpStatusCode.Unauthorized,
                { IsSuccess: false } and { Failure: not null } and { Failure: { FailureType: FailureType.Forbidden } } => (int)HttpStatusCode.Forbidden,
                _ => throw new NotSupportedException()
            };

            return new ApiResult
            {
                Value = value,
                StatusCode = statusCode,
            };
        }

        public async override Task ExecuteResultAsync(ActionContext context)
        {
            var executor = context.HttpContext.RequestServices.GetRequiredService<IActionResultExecutor<JsonResult>>();
            await executor.ExecuteAsync(context, new JsonResult(Value) { StatusCode = StatusCode }); //JsonOptionsConfiguration.Options
        }
    }

    public class ApiResult<T> : ActionResult, IStatusCodeActionResult, IActionResult
    {
        public Result<T>? Value { get; protected set; }

        public int? StatusCode { get; protected set; }

        public static implicit operator ApiResult<T>(Result<T> value)
        {
            if (value == null)
            {
                return new ApiResult<T>
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                };
            }

            var statusCode = value switch
            {
                { IsSuccess: true } => (int)HttpStatusCode.OK,
                { IsSuccess: false } and { Failure: not null } and { Failure: { FailureType: FailureType.ServerError } } => (int)HttpStatusCode.InternalServerError,
                { IsSuccess: false } and { Failure: not null } and { Failure: { FailureType: FailureType.ValidationError } } => (int)HttpStatusCode.BadRequest,
                { IsSuccess: false } and { Failure: not null } and { Failure: { FailureType: FailureType.NotFound } } => (int)HttpStatusCode.NotFound,
                { IsSuccess: false } and { Failure: not null } and { Failure: { FailureType: FailureType.Unauthorized } } => (int)HttpStatusCode.Unauthorized,
                { IsSuccess: false } and { Failure: not null } and { Failure: { FailureType: FailureType.Forbidden } } => (int)HttpStatusCode.Forbidden,
                _ => throw new NotSupportedException()
            };

            return new ApiResult<T>
            {
                Value = value,
                StatusCode = statusCode,
            };
        }

        public static implicit operator ApiResult<T>(Result value)
        {
            if (value == null)
            {
                return new ApiResult<T>
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                };
            }

            var statusCode = value switch
            {
                { IsSuccess: true } => (int)HttpStatusCode.OK,
                { IsSuccess: false } and { Failure: not null } and { Failure: { FailureType: FailureType.ServerError } } => (int)HttpStatusCode.InternalServerError,
                { IsSuccess: false } and { Failure: not null } and { Failure: { FailureType: FailureType.ValidationError } } => (int)HttpStatusCode.BadRequest,
                { IsSuccess: false } and { Failure: not null } and { Failure: { FailureType: FailureType.NotFound } } => (int)HttpStatusCode.NotFound,
                { IsSuccess: false } and { Failure: not null } and { Failure: { FailureType: FailureType.Unauthorized } } => (int)HttpStatusCode.Unauthorized,
                { IsSuccess: false } and { Failure: not null } and { Failure: { FailureType: FailureType.Forbidden } } => (int)HttpStatusCode.Forbidden,
                _ => throw new NotSupportedException()
            };

            return new ApiResult<T>
            {
                Value = value,
                StatusCode = statusCode,
            };
        }

        public async override Task ExecuteResultAsync(ActionContext context)
        {
            var executor = context.HttpContext.RequestServices.GetRequiredService<IActionResultExecutor<JsonResult>>();
            await executor.ExecuteAsync(context, new JsonResult(Value) { StatusCode = StatusCode });
        }
    }
}