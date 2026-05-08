using LanguagePlatform.Domain.Entities;

namespace LanguagePlatform.Domain.Interfaces;

public interface IProgressRepository : IGenericRepository<UserProgress>
{
    Task<UserProgress?> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<UserProgress>> GetTopLeaderboardAsync(int top = 10);
}

public interface INotificationRepository : IGenericRepository<Notification>
{
    Task<IEnumerable<Notification>> GetByUserIdAsync(Guid userId);
    Task MarkAllReadAsync(Guid userId);
}
