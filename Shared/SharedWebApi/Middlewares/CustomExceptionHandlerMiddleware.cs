using JO.Response.Base;

namespace SharedWebApi.Middlewares
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ArgumentNullException ex)
            {
                await HandleExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(new ApiResult(false, ApiResultStatusCode.ServerError, exception.Message));

            return context.Response.WriteAsync(json);
        }
    }
}
