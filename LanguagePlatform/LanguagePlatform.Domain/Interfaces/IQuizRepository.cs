using LanguagePlatform.Domain.Entities;

namespace LanguagePlatform.Domain.Interfaces;

public interface IQuizRepository : IGenericRepository<Quiz>
{
    Task<Quiz?> GetWithQuestionsAsync(Guid id);
    Task<IEnumerable<Quiz>> GetByLessonAsync(Guid lessonId);
    Task<IEnumerable<Quiz>> GetByGrammarTopicAsync(Guid grammarTopicId);
}
