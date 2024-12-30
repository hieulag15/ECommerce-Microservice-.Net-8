using eCommerce.SharedLibrary.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace eCommerce.SharedLibrary.Middleware;

public class GLodbalException(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        string message = "An error occurred while processing your request.";
        int statusCode = (int)HttpStatusCode.InternalServerError;
        string title = "Error";

        try
        {
            await next(context);

            // check if response is Too many requests // 429 status code
            if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
            {
                title = " Warning";
                message = "Too many requests, please try again later.";
                statusCode = StatusCodes.Status429TooManyRequests;
                await ModifyHeader(context, title, message, statusCode);
            }

            //  if resposne is unAuthorized // 401 status code
            if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                title = "Unauthorized";
                message = "You are not authorized to access this resource.";
                statusCode = StatusCodes.Status401Unauthorized;
                await ModifyHeader(context, title, message, statusCode);
            }

            // If response is Forbidden // 403 status code
            if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                title = "Forbidden";
                message = "You are not allowed to access this resource.";
                statusCode = StatusCodes.Status403Forbidden;
                await ModifyHeader(context, title, message, statusCode);
            }
        }
        catch (Exception ex)
        {
            // Log original exception
            LogExceptions.LogException(ex);

            // check if exception is timeout 
            if (ex is TaskCanceledException || ex is TimeoutException)
            {
                title = "Timeout";
                message = "Request timeout, please try again later.";
                statusCode = StatusCodes.Status408RequestTimeout;
            }
            await ModifyHeader(context, title, message, statusCode);

        }
    }

    private static async Task ModifyHeader(HttpContext context, string title, string message, int statusCode)
    {
        // display scary-free message to client
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(new ProblemDetails()
        {
            Detail = message,
            Title = title,
            Status = statusCode
        }), CancellationToken.None);
        return;
    }
}
