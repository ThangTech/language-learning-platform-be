using LanguagePlatform.Domain.Entities;
using LanguagePlatform.Domain.Enums;

namespace LanguagePlatform.Domain.Interfaces;

public interface IWordRepository : IGenericRepository<Word>
{
    Task<(IEnumerable<Word> Items, int TotalCount)> GetWordsPagedAsync(int page, int size, string? search = null, WordLevel? level = null, string? topic = null);
}
