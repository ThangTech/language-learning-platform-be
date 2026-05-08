using LanguagePlatform.Domain.Entities;
using LanguagePlatform.Domain.Enums;
using LanguagePlatform.Domain.Interfaces;
using LanguagePlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LanguagePlatform.Infrastructure.Repositories;

public class GrammarRepository : GenericRepository<GrammarTopic>, IGrammarRepository
{
    public GrammarRepository(AppDbContext context) : base(context) { }

    public async Task<(IEnumerable<GrammarTopic> Items, int TotalCount)> GetTopicsPagedAsync(
        int page, int pageSize, GrammarLevel? level = null, string? search = null)
    {
        var query = _dbSet.AsQueryable();
        if (level.HasValue)
            query = query.Where(g => g.Level == level.Value);
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(g => g.Title.Contains(search));

        var total = await query.CountAsync();
        var items = await query
            .OrderBy(g => g.Title)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (items, total);
    }
}

public class UserGrammarRepository : GenericRepository<UserGrammar>, IUserGrammarRepository
{
    public UserGrammarRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<UserGrammar>> GetByUserIdAsync(Guid userId)
        => await _dbSet.Include(ug => ug.Topic).Where(ug => ug.UserId == userId).ToListAsync();

    public async Task<UserGrammar?> GetByUserAndTopicAsync(Guid userId, Guid topicId)
        => await _dbSet.FirstOrDefaultAsync(ug => ug.UserId == userId && ug.TopicId == topicId);
}
