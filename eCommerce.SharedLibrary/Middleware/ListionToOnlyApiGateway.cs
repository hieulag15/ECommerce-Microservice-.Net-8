using Microsoft.AspNetCore.Http;

namespace eCommerce.SharedLibrary.Middleware
{
    public class ListionToOnlyApiGateway(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            // Extract specific header from request
            var singnedHeader = context.Request.Headers["Api-Gateway"];

            // Null means, the request is not coming from API Gateway // 503 service unavailable
            if (singnedHeader.FirstOrDefault() is null)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                context.Response.WriteAsync("Service Unavailable");
                return;
            }
            else
            {
                await next(context);
            }
        }
    }
}
