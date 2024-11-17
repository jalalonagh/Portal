using JO.Tools.Extensions;

namespace JO.Response.Base
{
    public class ApiResult
    {
        public bool IsSuccess { get; set; }
        public ApiResultStatusCode StatusCode { get; set; }
        public string? Message { get; set; }

        public ApiResult(bool isSuccess, ApiResultStatusCode statusCode)
        {
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            Message = statusCode.ToDisplay();
        }
        
        public ApiResult(bool isSuccess, ApiResultStatusCode statusCode, string message)
        {
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            Message = message ?? statusCode.ToDisplay();
        }
    }

    public class ApiResult<TData> : ApiResult
        where TData : class
    {
        public TData? Data { get; set; }

        public ApiResult(bool isSuccess, ApiResultStatusCode statusCode, TData? data)
            : base(isSuccess, statusCode, statusCode.ToDisplay())
        {
            Data = data;
        }

        public ApiResult(bool isSuccess, ApiResultStatusCode statusCode, TData? data, string message)
            : base(isSuccess, statusCode, message)
        {
            Data = data;
        }

        #region Implicit Operators
        public static implicit operator ApiResult<TData>(TData data)
        {
            return new ApiResult<TData>(data != null, data != null ? ApiResultStatusCode.Success : ApiResultStatusCode.ServerError, data);
        }
        #endregion
    }
}
