using LanguagePlatform.Application.DTOs.Common;
using LanguagePlatform.Application.DTOs.Vocabulary;

namespace LanguagePlatform.Application.Interfaces;

public interface IVocabularyService
{
    // Words
    Task<ApiResponse<PagedResult<WordDto>>> GetWordsAsync(int page, int pageSize, string? level = null, string? search = null);
    Task<ApiResponse<WordDto>> GetWordByIdAsync(Guid id);
    Task<ApiResponse<WordDto>> CreateWordAsync(CreateWordRequest request);
    Task<ApiResponse<WordDto>> UpdateWordAsync(Guid id, UpdateWordRequest request);
    Task<ApiResponse<bool>> DeleteWordAsync(Guid id);

    // Favorites
    Task<ApiResponse<IEnumerable<WordDto>>> GetFavoritesAsync(Guid userId);
    Task<ApiResponse<bool>> AddFavoriteAsync(Guid userId, Guid wordId);
    Task<ApiResponse<bool>> RemoveFavoriteAsync(Guid userId, Guid wordId);

    // Flashcards
    Task<ApiResponse<IEnumerable<FlashcardDto>>> GetFlashcardsAsync(Guid userId);
    Task<ApiResponse<FlashcardDto>> AddToFlashcardAsync(Guid userId, Guid wordId);
    Task<ApiResponse<bool>> MarkFlashcardLearnedAsync(Guid userId, Guid wordId);
    Task<ApiResponse<bool>> RemoveFlashcardAsync(Guid userId, Guid wordId);
}
