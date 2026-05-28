using System.Security.Claims;
using LanguagePlatform.Application.DTOs.Notification;
using LanguagePlatform.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanguagePlatform.API.Controllers;

// Controller quản lý thông báo trong ứng dụng
[ApiController]
[Route("api/notifications")]
[Authorize]
public class NotificationsController : ApiControllerBase
{
    private readonly INotificationService _notifService;

    public NotificationsController(INotificationService notifService)
    {
        _notifService = notifService;
    }

    // Lấy tất cả thông báo của người dùng đang đăng nhập
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        Guid userId = GetUserId();
        var result = await _notifService.GetUserNotificationsAsync(userId);
        return HandleResult(result);
    }

    // Tạo thông báo mới - chỉ Admin được phép
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateNotificationRequest request)
    {
        var result = await _notifService.CreateNotificationAsync(request);
        return HandleResult(result);
    }

    // Đánh dấu một thông báo là đã đọc
    [HttpPut("{id:guid}/read")]
    public async Task<IActionResult> MarkRead(Guid id)
    {
        Guid userId = GetUserId();
        var result = await _notifService.MarkAsReadAsync(userId, id);
        return HandleResult(result);
    }

    // Đánh dấu tất cả thông báo là đã đọc
    [HttpPut("read-all")]
    public async Task<IActionResult> MarkAllRead()
    {
        Guid userId = GetUserId();
        var result = await _notifService.MarkAllAsReadAsync(userId);
        return HandleResult(result);
    }

    // Xóa một thông báo
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        Guid userId = GetUserId();
        var result = await _notifService.DeleteNotificationAsync(userId, id);
        return HandleResult(result);
    }
}
