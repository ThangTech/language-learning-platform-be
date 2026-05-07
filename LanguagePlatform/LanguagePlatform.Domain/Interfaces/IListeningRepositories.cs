using LanguagePlatform.Domain.Entities;

namespace LanguagePlatform.Domain.Interfaces;

public interface IListeningRepository : IGenericRepository<ListeningLesson>
{
    Task<(IEnumerable<ListeningLesson> Items, int TotalCount)> GetLessonsPagedAsync(int page, int size, string? level = null, string? topic = null);
    Task<ListeningLesson?> GetWithQuizzesAsync(Guid id);
}

public interface IDictationRepository : IGenericRepository<DictationSet>
{
    Task<DictationSet?> GetWithSentencesAsync(Guid id);
    Task<IEnumerable<DictationSet>> GetByLessonAsync(Guid lessonId);
}

public interface IListeningResultRepository : IGenericRepository<ListeningResult>
{
    Task<IEnumerable<ListeningResult>> GetByUserIdAsync(Guid userId);
}
