using JO.Tools.Extensions;

namespace JO.Response.Base
{
    public class ApiResult
    {
        public bool IsSuccess { get; set; }
        public ApiResultStatusCode StatusCode { get; set; }
        public string? Message { get; set; }

        public ApiResult(bool isSuccess, ApiResultStatusCode statusCode, string? message = null)
        {
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            Message = message ?? statusCode.ToDisplay();
        }

        public static ApiResult NotFound(string message)
        {
            return new ApiResult(false, ApiResultStatusCode.NotFound, message);
        }

        public static ApiResult OperationFailed(string message)
        {
            return new ApiResult(false, ApiResultStatusCode.ServerError, message);
        }

        public static ApiResult BusinessFailed(string message)
        {
            return new ApiResult(false, ApiResultStatusCode.LogicError, message);
        }

        public static ApiResult Success(string? message)
        {
            return new ApiResult(true, ApiResultStatusCode.Success, message);
        }

        public static ApiResult Invalid(string message)
        {
            return new ApiResult(false, ApiResultStatusCode.BadRequest, message);
        }
    }

    public class ApiResult<TData> : ApiResult
        where TData : class
    {
        public TData? Data { get; set; }

        public ApiResult(bool isSuccess, ApiResultStatusCode statusCode, TData? data, string? message = null)
            : base(isSuccess, statusCode, message)
        {
            Data = data;
        }

        public static ApiResult<TData> NotFound(string message, TData? data = null)
        {
            return new ApiResult<TData>(false, ApiResultStatusCode.NotFound, data, message);
        }

        public static ApiResult<TData> OperationFailed(string message, TData? data = null)
        {
            return new ApiResult<TData>(false, ApiResultStatusCode.ServerError, data, message);
        }

        public static ApiResult<TData> BusinessFailed(string message, TData? data = null)
        {
            return new ApiResult<TData>(false, ApiResultStatusCode.LogicError, data, message);
        }

        public static ApiResult<TData> Success(TData data, string? message = null)
        {
            return new ApiResult<TData>(true, ApiResultStatusCode.Success, data, message);
        }

        public static ApiResult<TData> Invalid(string message)
        {
            return new ApiResult<TData>(false, ApiResultStatusCode.BadRequest, null, message);
        }

        #region Implicit Operators
        public static implicit operator ApiResult<TData>(TData data)
        {
            return new ApiResult<TData>(data != null, data != null ? ApiResultStatusCode.Success : ApiResultStatusCode.ServerError, data);
        }
        #endregion
    }
}
