using LanguagePlatform.Application.DTOs.Common;
using LanguagePlatform.Application.DTOs.Notification;

namespace LanguagePlatform.Application.Interfaces;

public interface INotificationService
{
    Task<ApiResponse<IEnumerable<NotificationDto>>> GetUserNotificationsAsync(Guid userId);
    Task<ApiResponse<NotificationDto>> CreateNotificationAsync(CreateNotificationRequest request);
    Task<ApiResponse<bool>> MarkAsReadAsync(Guid userId, Guid notificationId);
    Task<ApiResponse<bool>> MarkAllAsReadAsync(Guid userId);
    Task<ApiResponse<bool>> DeleteNotificationAsync(Guid userId, Guid notificationId);
}
