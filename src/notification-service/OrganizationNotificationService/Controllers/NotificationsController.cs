using Microsoft.AspNetCore.Mvc;
using OrganizationNotificationService.Features.SendNotification;
using OrganizationNotificationService.Features.SendNotification.Models;
using OrganizationNotificationService.Models;

namespace OrganizationNotificationService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
   [HttpGet]
   public async Task<ActionResult<Notification>> GetNotificationStatus([FromQuery] Guid notificationId)
   {
      //TODO: Implement an api here that lets clients query for notification
      return Ok();
   }
}