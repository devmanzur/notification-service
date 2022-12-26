using Microsoft.AspNetCore.Mvc;
using OrganizationNotificationService.Exceptions;
using OrganizationNotificationService.Features.SendNotification;
using OrganizationNotificationService.Models;
using OrganizationNotificationService.Utils;

namespace OrganizationNotificationService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly NotificationService _notificationService;
    private readonly ILogger<NotificationsController> _logger;

    public NotificationsController(NotificationService notificationService,ILogger<NotificationsController> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    /// <summary>
    /// Returns the notification status and details if exists
    /// </summary>
    /// <param name="notificationId"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<Envelope<ApplicationNotification>>> GetNotificationStatus(
        [FromQuery] Guid notificationId)
    {
        var notification = await _notificationService.GetNotificationStatus(notificationId);
        if (notification != null)
        {
            return Ok(Envelope.Ok(notification));
        }

        return NotFound();
    }

    /// <summary>
    /// Creates a new notification
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<Envelope<NotificationResponse>>> CreateNotification(
        [FromBody] NotificationRequest request)
    {
        try
        {
            RuleValidator.Validate<NotificationRequest, NotificationRequestValidator>(request);
            var createNotification = request.GetType() switch
            {
                NotificationType.Email => await _notificationService.AddNewEmailNotification(request),
                NotificationType.PushNotification => await _notificationService.AddNewPushNotification(request),
                _ => throw new DomainValidationException("Invalid Notification type",
                    "Attempted to create unsupported notification type")
            };
            return Ok(Envelope.Ok(createNotification));
        }
        catch (DomainValidationException e)
        {
            _logger.LogError(e,"Failed to add notification due to validation errors");
            return BadRequest(Envelope.Error(e));
        }
    }
}