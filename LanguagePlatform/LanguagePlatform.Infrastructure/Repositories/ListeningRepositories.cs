using LanguagePlatform.Domain.Entities;
using LanguagePlatform.Domain.Interfaces;
using LanguagePlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LanguagePlatform.Infrastructure.Repositories;

public class ListeningRepository : GenericRepository<ListeningLesson>, IListeningRepository
{
    public ListeningRepository(AppDbContext context) : base(context) { }

    public async Task<(IEnumerable<ListeningLesson> Items, int TotalCount)> GetLessonsPagedAsync(
        int page, int pageSize, string? level = null, string? search = null)
    {
        var query = _dbSet.AsQueryable();
        if (!string.IsNullOrWhiteSpace(level))
            query = query.Where(l => l.Level == level);
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(l => l.Title.Contains(search) || l.Topic.Contains(search));

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(l => l.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (items, total);
    }
}

public class ListeningResultRepository : GenericRepository<ListeningResult>, IListeningResultRepository
{
    public ListeningResultRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<ListeningResult>> GetByUserIdAsync(Guid userId)
        => await _dbSet.Include(r => r.Lesson).Where(r => r.UserId == userId).ToListAsync();
}

public class DictationSetRepository : GenericRepository<DictationSet>, IDictationSetRepository
{
    public DictationSetRepository(AppDbContext context) : base(context) { }

    public async Task<DictationSet?> GetWithSentencesAsync(Guid id)
        => await _dbSet.Include(d => d.Sentences).FirstOrDefaultAsync(d => d.Id == id);

    public async Task<IEnumerable<DictationSet>> GetAllWithSentencesAsync()
        => await _dbSet.Include(d => d.Sentences).ToListAsync();
}
