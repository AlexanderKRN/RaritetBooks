using RaritetBooks.API.Common;
using RaritetBooks.Domain.Common;
using System.Net;

namespace RaritetBooks.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);

            var errorInfo = new ErrorInfo(ErrorList.General.Internal(e.Message));
            var envelope = Envelope.Error([errorInfo]);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync(envelope);
        }
    }
}