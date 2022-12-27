using System.Net;
using OrganizationNotificationService.Models;
using OrganizationNotificationService.Utils;

namespace OrganizationNotificationService.Middlewares;

public class ExceptionFormattingMiddleware : IFactoryMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<ExceptionFormattingMiddleware> _logger;

    public ExceptionFormattingMiddleware(RequestDelegate next, IWebHostEnvironment env,
        ILoggerFactory loggerFactory)
    {
        _next = next;
        _env = env;
        _logger = loggerFactory
            .CreateLogger<ExceptionFormattingMiddleware>();
    }

    private Task HandleException(HttpContext context, Exception exception, IWebHostEnvironment env)
    {
        try
        {
            //only expecting 500 server exceptions since we are not throwing any other type of exceptions

            var error = env.IsDevelopment()
                ? exception.Message
                : "Something went wrong, please try again or contact support";

            _logger.LogError(exception, message:
                "Http Request Exception Information: {Environment} Schema:{Schema} Host: {Host} Path: {Path} QueryString: {QueryString}  Error Message: {ErrorMessage} Error Trace: {StackTrace}, ToString: {ToString}",
                Environment.NewLine, context.Request.Scheme, context.Request.Host, context.Request.Path,
                context.Request.QueryString, exception.Message, exception.StackTrace, exception.ToString());


            var response = Envelope.Error(error);
            var result = AppJsonUtils.Serialize(response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception in middleware task");

            var response = Envelope.Error("Something went wrong");
            var result = AppJsonUtils.Serialize(response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(result);
        }
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex, _env);
        }
    }
}