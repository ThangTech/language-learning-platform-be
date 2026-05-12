namespace LanguagePlatform.Application.DTOs.Notification;

public class NotificationDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public bool IsRead { get; set; }

    public string Type { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public string TimeAgo
    {
        get
        {
            return GetTimeAgo();
        }
    }

    private string GetTimeAgo()
    {
        var now = DateTime.UtcNow;
        var diff = now - CreatedAt;

        if (diff.TotalMinutes < 1)
        {
            return "Vừa xong";
        }

        if (diff.TotalMinutes < 60)
        {
            return $"{(int)diff.TotalMinutes} phút trước";
        }

        if (diff.TotalHours < 24)
        {
            return $"{(int)diff.TotalHours} giờ trước";
        }

        if (diff.TotalDays < 7)
        {
            return $"{(int)diff.TotalDays} ngày trước";
        }

        return CreatedAt.ToString("dd/MM/yyyy");
    }
}

public class CreateNotificationRequest
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = "System";
}
