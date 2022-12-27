namespace OrganizationNotificationService.Middlewares;

public interface IFactoryMiddleware
{
    Task InvokeAsync(HttpContext context);
}