namespace Shared.Models.OperationResults
{
    public class Result : BaseResult
    {
        public static Result Success()
        {
            return new Result
            {
                IsSuccess = true,
                Failure = default,
            };
        }

        public static Result ServerError(string message)
        {
            return new Result
            {
                IsSuccess = false,
                Failure = new FailureInfo
                {
                    Message = message,
                    FailureType = FailureType.ServerError
                }
            };
        }

        public static Result ValidationError(ValidationMessage validationError)
        {
            return new Result
            {
                IsSuccess = false,
                Failure = new FailureInfo
                {
                    ValidationMessages = new List<ValidationMessage> { validationError },
                    FailureType = FailureType.ValidationError
                }
            };
        }

        public static Result ValidationError(IEnumerable<ValidationMessage> validationErrors)
        {
            return new Result
            {
                IsSuccess = false,
                Failure = new FailureInfo
                {
                    ValidationMessages = validationErrors?.ToList(),
                    FailureType = FailureType.ValidationError
                }
            };
        }

        public static Result NotFound(string message)
        {
            return new Result
            {
                IsSuccess = false,
                Failure = new FailureInfo
                {
                    Message = message,
                    FailureType = FailureType.NotFound
                }
            };
        }

        public static Result Unauthorized(string message)
        {
            return new Result
            {
                IsSuccess = false,
                Failure = new FailureInfo
                {
                    Message = message,
                    FailureType = FailureType.Unauthorized
                }
            };
        }

        public static Result Forbidden(string message)
        {
            return new Result
            {
                IsSuccess = false,
                Failure = new FailureInfo
                {
                    Message = message,
                    FailureType = FailureType.Forbidden
                }
            };
        }
    }

    public class Result<T> : BaseResult
    {
        public T? Data { get; set; }

        public static implicit operator Result<T>(T value)
        {
            return new Result<T>
            {
                Data = value,
                IsSuccess = true
            };
        }

        public static implicit operator Result<T>(Result result)
        {
            if (result == null)
            {
                return new Result<T>();
            }

            return new Result<T>
            {
                IsSuccess = result.IsSuccess,
                Failure = result.Failure,
                Data = default,
            };
        }

        public static explicit operator Result(Result<T> result)
        {
            if (result == null)
            {
                return new Result();
            }

            return new Result
            {
                IsSuccess = result.IsSuccess,
                Failure = result.Failure,
            };
        }
    }
}
