using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using JO.Response.Base;
using JO.Shared.Interfaces;

namespace SharedWebApi.AuthorizeValidation
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class BaseAuthorize : Attribute, IAuthorizationFilter
    {
        private readonly string _AccessId;

        public BaseAuthorize()
        {

        }

        public BaseAuthorize(string accessId)
        {
            _AccessId = accessId;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var service = context.HttpContext.RequestServices.GetService<ITokenService>();

            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
            {
                return;
            }

            string token = (context.HttpContext?.Request?.Headers["Authorization"].FirstOrDefault() ?? "").Split(" ").Last();

            if (!string.IsNullOrEmpty(token))
            {
                context.HttpContext.User = service.Validate(token);
            }

            if (!(context.HttpContext.User.Identity?.IsAuthenticated ?? false))
            {
                var result = new ObjectResult(new ApiResult(false, ApiResultStatusCode.Forbidden, "به این بخش دسترسی ندارید"));
                result.StatusCode = 403;
                context.Result = result;
            }
        }
    }
}
