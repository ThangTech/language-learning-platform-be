using LanguagePlatform.Domain.Common;
using LanguagePlatform.Domain.Enums;

namespace LanguagePlatform.Domain.Entities;

public class Notification : BaseEntity
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; } = false;
    public NotificationType Type { get; set; } = NotificationType.System;

    // Navigation
    public User User { get; set; } = null!;
}
