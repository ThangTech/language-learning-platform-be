using LanguagePlatform.Domain.Entities;

namespace LanguagePlatform.Domain.Interfaces;

public interface IFavoriteRepository : IGenericRepository<Favorite>
{
    Task<IEnumerable<Favorite>> GetByUserIdAsync(Guid userId);
    Task<Favorite?> GetByUserAndWordAsync(Guid userId, Guid wordId);
}

public interface IFlashcardRepository : IGenericRepository<Flashcard>
{
    Task<IEnumerable<Flashcard>> GetByUserIdAsync(Guid userId);
    Task<Flashcard?> GetByUserAndWordAsync(Guid userId, Guid wordId);
    Task<IEnumerable<Flashcard>> GetUnlearnedAsync(Guid userId);
}
