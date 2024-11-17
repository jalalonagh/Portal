namespace SharedWebApi.Middlewares
{
    public class JWTMiddleware
    {
        private readonly RequestDelegate _next;

        public JWTMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
#if RELEASE  // DEBUG
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var principal = tokenService.Validate(token);
            context.User = principal;

            // SOME BLOCKS OR MANAGEMENTS CAN WRITE HERE
            // .........................................
            // END
#endif
            return _next(context);
        }
    }
}
