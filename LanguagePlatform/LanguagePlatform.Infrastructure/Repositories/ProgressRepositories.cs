using LanguagePlatform.Domain.Entities;
using LanguagePlatform.Domain.Interfaces;
using LanguagePlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LanguagePlatform.Infrastructure.Repositories;

public class ProgressRepository : GenericRepository<UserProgress>, IProgressRepository
{
    public ProgressRepository(AppDbContext context) : base(context) { }

    public async Task<UserProgress?> GetByUserIdAsync(Guid userId)
        => await _dbSet.Include(p => p.User).FirstOrDefaultAsync(p => p.UserId == userId);

    public async Task<IEnumerable<UserProgress>> GetTopLeaderboardAsync(int top)
        => await _dbSet.Include(p => p.User)
            .OrderByDescending(p => p.TotalScore)
            .Take(top)
            .ToListAsync();
}

public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
{
    public NotificationRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Notification>> GetByUserIdAsync(Guid userId)
        => await _dbSet.Where(n => n.UserId == userId).OrderByDescending(n => n.CreatedAt).ToListAsync();

    public async Task MarkAllReadAsync(Guid userId)
    {
        var notifs = await _dbSet.Where(n => n.UserId == userId && !n.IsRead).ToListAsync();
        notifs.ForEach(n => n.IsRead = true);
        await _context.SaveChangesAsync();
    }
}
