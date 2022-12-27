using Microsoft.IO;

namespace OrganizationNotificationService.Middlewares;

public class RequestResponseLoggingMiddleware : IFactoryMiddleware

{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

    private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;


    public RequestResponseLoggingMiddleware(ILoggerFactory loggerFactory, RequestDelegate next)

    {
        _next = next;
        _logger = loggerFactory.CreateLogger<RequestResponseLoggingMiddleware>();
        _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
    }


    public async Task InvokeAsync(HttpContext context)
    {
        await LogRequest(context);
        var start = DateTime.UtcNow;
        await _next(context);
        await LogResponse(context, start);
    }

    private async Task LogRequest(HttpContext context)

    {
        try
        {
            context.Request.EnableBuffering();
            await using var requestStream = _recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);

            _logger.LogInformation(message:
                "Http Request Information: {Environment}  Schema:{Schema} Host: {Host} Path: {Path}, QueryString: {QueryString}, Request Body: {RequestBody}",
                Environment.NewLine, context.Request.Scheme, context.Request.Host, context.Request.Path,
                context.Request.QueryString, ReadStreamInChunks(requestStream));

            context.Request.Body.Position = 0;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception in middleware task");
        }
    }


    private static string ReadStreamInChunks(Stream stream)

    {
        const int readChunkBufferLength = 4096;
        stream.Seek(0, SeekOrigin.Begin);
        using var textWriter = new StringWriter();
        using var reader = new StreamReader(stream);
        var readChunk = new char[readChunkBufferLength];
        int readChunkLength;

        do
        {
            readChunkLength = reader.ReadBlock(readChunk,
                0,
                readChunkBufferLength);

            textWriter.Write(readChunk, 0, readChunkLength);
        } while (readChunkLength > 0);
        
        return textWriter.ToString();
    }


    private async Task LogResponse(HttpContext context, DateTime start)

    {
        try
        {
            var originalBodyStream = context.Response.Body;
            await using var responseBody = _recyclableMemoryStreamManager.GetStream();
            context.Response.Body = responseBody;
            var end = DateTime.UtcNow;
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            _logger.LogInformation(
                "StatusCode: {StatusCode}, Http Response Information: {Environment}   Schema:{Schema} Host: {Host} Path: {Path} QueryString: {QueryString} ElapsedTime: {ElapsedTime} Response Body: {ResponseBody}",
                context.Response.StatusCode, Environment.NewLine, context.Request.Scheme, context.Request.Host,
                context.Request.Path,
                context.Request.QueryString, (end - start).TotalMilliseconds, text);

            await responseBody.CopyToAsync(originalBodyStream);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception in middleware task");
        }
    }
}