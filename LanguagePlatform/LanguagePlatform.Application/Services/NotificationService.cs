using AutoMapper;
using LanguagePlatform.Application.DTOs.Common;
using LanguagePlatform.Application.DTOs.Notification;
using LanguagePlatform.Application.Interfaces;
using LanguagePlatform.Domain.Interfaces;

namespace LanguagePlatform.Application.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notifRepo;
    private readonly IMapper _mapper;

    public NotificationService(INotificationRepository notifRepo, IMapper mapper) { _notifRepo = notifRepo; _mapper = mapper; }

    public async Task<ApiResponse<IEnumerable<NotificationDto>>> GetUserNotificationsAsync(Guid userId)
        => ApiResponse<IEnumerable<NotificationDto>>.Ok(_mapper.Map<List<NotificationDto>>(await _notifRepo.GetByUserIdAsync(userId)));

    public async Task<ApiResponse<NotificationDto>> CreateNotificationAsync(CreateNotificationRequest request)
    {
        var notif = _mapper.Map<Domain.Entities.Notification>(request);
        await _notifRepo.AddAsync(notif);
        await _notifRepo.SaveChangesAsync();
        return ApiResponse<NotificationDto>.Ok(_mapper.Map<NotificationDto>(notif), "Đã tạo thông báo.");
    }

    public async Task<ApiResponse<bool>> MarkAsReadAsync(Guid userId, Guid notificationId)
    {
        var notif = await _notifRepo.GetByIdAsync(notificationId);
        if (notif == null || notif.UserId != userId) return ApiResponse<bool>.Fail("Không tìm thấy thông báo.");
        notif.IsRead = true;
        _notifRepo.Update(notif);
        await _notifRepo.SaveChangesAsync();
        return ApiResponse<bool>.Ok(true, "Đã đánh dấu đã đọc.");
    }

    public async Task<ApiResponse<bool>> MarkAllAsReadAsync(Guid userId)
    {
        await _notifRepo.MarkAllReadAsync(userId);
        return ApiResponse<bool>.Ok(true, "Đã đánh dấu tất cả đã đọc.");
    }

    public async Task<ApiResponse<bool>> DeleteNotificationAsync(Guid userId, Guid notificationId)
    {
        var notif = await _notifRepo.GetByIdAsync(notificationId);
        if (notif == null || notif.UserId != userId) return ApiResponse<bool>.Fail("Không tìm thấy thông báo.");
        _notifRepo.Delete(notif);
        await _notifRepo.SaveChangesAsync();
        return ApiResponse<bool>.Ok(true, "Đã xóa thông báo.");
    }
}
