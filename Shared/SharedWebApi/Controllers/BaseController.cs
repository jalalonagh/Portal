using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using SharedWebApi.AuthorizeValidation;
using System.Security.Claims;

namespace SharedWebApi.Controllers
{
    [ApiController]
    [RequestTimeout(10000)]
    [BaseAuthorize()]
    public class BaseApiController : ControllerBase
    {
        public long UserId => long.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        public long ProfileId => long.Parse(HttpContext.User.FindFirstValue("UserProfileId") ?? "0");
        public string Username => HttpContext.User.FindFirstValue(ClaimTypes.Name) ?? "";
    }
}
