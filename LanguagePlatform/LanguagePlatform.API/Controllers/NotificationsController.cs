using System.Security.Claims;
using LanguagePlatform.Application.DTOs.Notification;
using LanguagePlatform.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanguagePlatform.API.Controllers;

[ApiController]
[Route("api/notifications")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notifService;
    public NotificationsController(INotificationService notifService) => _notifService = notifService;

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _notifService.GetUserNotificationsAsync(GetUserId()));

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateNotificationRequest request)
        => Ok(await _notifService.CreateNotificationAsync(request));

    [HttpPut("{id:guid}/read")]
    public async Task<IActionResult> MarkRead(Guid id)
        => Ok(await _notifService.MarkAsReadAsync(GetUserId(), id));

    [HttpPut("read-all")]
    public async Task<IActionResult> MarkAllRead()
        => Ok(await _notifService.MarkAllAsReadAsync(GetUserId()));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
        => Ok(await _notifService.DeleteNotificationAsync(GetUserId(), id));

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
