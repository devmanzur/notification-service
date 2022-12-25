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

    [HttpGet]
    public async Task<ActionResult<Envelope<ApplicationNotification>>> GetNotificationStatus([FromQuery] Guid notificationId)
    {
        //TODO: Implement an api here that lets clients query for notification
        return Ok();
    }

    [HttpPost("email")]
    public async Task<ActionResult<ApplicationNotification>> CreateEmailNotification([FromBody] NotificationRequest request)
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
    public async Task<ActionResult<Envelope<ApplicationNotification>>> CreatePushNotification(
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