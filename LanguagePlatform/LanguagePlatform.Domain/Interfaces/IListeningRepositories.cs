using LanguagePlatform.Domain.Entities;

namespace LanguagePlatform.Domain.Interfaces;

public interface IListeningRepository : IGenericRepository<ListeningLesson>
{
    Task<(IEnumerable<ListeningLesson> Items, int TotalCount)> GetLessonsPagedAsync(int page, int pageSize, string? level = null, string? search = null);
}

public interface IDictationSetRepository : IGenericRepository<DictationSet>
{
    Task<DictationSet?> GetWithSentencesAsync(Guid id);
    Task<IEnumerable<DictationSet>> GetAllWithSentencesAsync();
}

public interface IListeningResultRepository : IGenericRepository<ListeningResult>
{
    Task<IEnumerable<ListeningResult>> GetByUserIdAsync(Guid userId);
}
