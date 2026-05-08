using LanguagePlatform.Domain.Entities;
using LanguagePlatform.Domain.Enums;
using LanguagePlatform.Domain.Interfaces;
using LanguagePlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LanguagePlatform.Infrastructure.Repositories;

public class WordRepository : GenericRepository<Word>, IWordRepository
{
    public WordRepository(AppDbContext context) : base(context) { }

    public async Task<(IEnumerable<Word> Items, int TotalCount)> GetWordsPagedAsync(
        int page, int pageSize, WordLevel? level = null, string? search = null)
    {
        var query = _dbSet.AsQueryable();
        if (level.HasValue)
            query = query.Where(w => w.Level == level.Value);
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(w => w.Term.Contains(search) || w.Definition.Contains(search));

        var total = await query.CountAsync();
        var items = await query
            .OrderBy(w => w.Term)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (items, total);
    }
}

public class FavoriteRepository : GenericRepository<Favorite>, IFavoriteRepository
{
    public FavoriteRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Favorite>> GetByUserIdAsync(Guid userId)
        => await _dbSet.Include(f => f.Word).Where(f => f.UserId == userId).ToListAsync();

    public async Task<Favorite?> GetByUserAndWordAsync(Guid userId, Guid wordId)
        => await _dbSet.FirstOrDefaultAsync(f => f.UserId == userId && f.WordId == wordId);
}

public class FlashcardRepository : GenericRepository<Flashcard>, IFlashcardRepository
{
    public FlashcardRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Flashcard>> GetByUserIdAsync(Guid userId)
        => await _dbSet.Include(f => f.Word).Where(f => f.UserId == userId).ToListAsync();

    public async Task<Flashcard?> GetByUserAndWordAsync(Guid userId, Guid wordId)
        => await _dbSet.FirstOrDefaultAsync(f => f.UserId == userId && f.WordId == wordId);

    public async Task<IEnumerable<Flashcard>> GetUnlearnedAsync(Guid userId)
        => await _dbSet.Include(f => f.Word).Where(f => f.UserId == userId && !f.IsLearned).ToListAsync();
}
