using LanguagePlatform.Domain.Entities;

namespace LanguagePlatform.Domain.Interfaces;

public interface IQuizResultRepository : IGenericRepository<QuizResult>
{
    Task<IEnumerable<QuizResult>> GetByUserIdAsync(Guid userId);
}
