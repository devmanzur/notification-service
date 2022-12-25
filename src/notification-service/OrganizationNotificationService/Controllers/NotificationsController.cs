using Microsoft.AspNetCore.Mvc;
using OrganizationNotificationService.Exceptions;
using OrganizationNotificationService.Features.AddNotification;
using OrganizationNotificationService.Models;
using OrganizationNotificationService.Utils;

namespace OrganizationNotificationService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly NotificationService _notificationService;

    public NotificationsController(NotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    /// <summary>
    /// Returns the notification status and details if exists
    /// </summary>
    /// <param name="notificationId"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<Envelope<ApplicationNotification>>> GetNotificationStatus([FromQuery] Guid notificationId)
    {
        var notification = await _notificationService.GetNotificationStatus(notificationId);
        if (notification != null)
        {
            return Ok(Envelope.Ok(notification));
        }

        return NotFound();
    }

    /// <summary>
    /// Creates a new email notification
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("email")]
    public async Task<ActionResult<Envelope<NotificationResponse>>> CreateEmailNotification([FromBody] NotificationRequest request)
    {
        try
        {
            RuleValidator.Validate<NotificationRequest, NotificationRequestValidator>(request);
            var createNotification = await _notificationService.AddNewEmailNotification(request);
            return Ok(Envelope.Ok(createNotification));
        }
        catch (DomainValidationException e)
        {
            return BadRequest(Envelope.Error(e));
        }
    }

    [HttpPost("push")]
    public async Task<ActionResult<Envelope<NotificationResponse>>> CreatePushNotification(
        [FromBody] NotificationRequest request)
    {
        try
        {
            RuleValidator.Validate<NotificationRequest, NotificationRequestValidator>(request);
            var createNotification = await _notificationService.AddNewPushNotification(request);
            return Ok(Envelope.Ok(createNotification));
        }
        catch (DomainValidationException e)
        {
            return BadRequest(Envelope.Error(e));
        }
    }
}