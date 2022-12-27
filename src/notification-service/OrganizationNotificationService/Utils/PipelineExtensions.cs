using OrganizationNotificationService.Middlewares;

namespace OrganizationNotificationService.Utils;

public static class PipelineExtensions
{
    public static IApplicationBuilder UseFactoryMiddleware<T>(this IApplicationBuilder builder) where T : IFactoryMiddleware
    {
        builder.UseMiddleware<T>();
        return builder;
    }
}