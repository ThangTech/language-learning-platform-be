using AutoMapper;
using LanguagePlatform.Application.DTOs.Common;
using LanguagePlatform.Application.DTOs.Vocabulary;
using LanguagePlatform.Application.Interfaces;
using LanguagePlatform.Domain.Entities;
using LanguagePlatform.Domain.Enums;
using LanguagePlatform.Domain.Interfaces;

namespace LanguagePlatform.Application.Services;

public class VocabularyService : IVocabularyService
{
    private readonly IWordRepository _wordRepo;
    private readonly IFavoriteRepository _favoriteRepo;
    private readonly IFlashcardRepository _flashcardRepo;
    private readonly IMapper _mapper;

    public VocabularyService(
        IWordRepository wordRepo,
        IFavoriteRepository favoriteRepo,
        IFlashcardRepository flashcardRepo,
        IMapper mapper)
    {
        _wordRepo = wordRepo;
        _favoriteRepo = favoriteRepo;
        _flashcardRepo = flashcardRepo;
        _mapper = mapper;
    }

    // ── Words ────────────────────────────────────────────────────────────────
    public async Task<ApiResponse<PagedResult<WordDto>>> GetWordsAsync(
        int page, int pageSize, string? level = null, string? search = null)
    {
        WordLevel? wordLevel = null;
        if (!string.IsNullOrWhiteSpace(level) && Enum.TryParse<WordLevel>(level, true, out var parsed))
            wordLevel = parsed;

        var (items, total) = await _wordRepo.GetWordsPagedAsync(page, pageSize, wordLevel, search);
        return ApiResponse<PagedResult<WordDto>>.Ok(new PagedResult<WordDto>
        {
            Items = _mapper.Map<List<WordDto>>(items),
            TotalCount = total,
            Page = page,
            PageSize = pageSize
        });
    }

    public async Task<ApiResponse<WordDto>> GetWordByIdAsync(Guid id)
    {
        var word = await _wordRepo.GetByIdAsync(id);
        if (word == null) return ApiResponse<WordDto>.Fail("Không tìm thấy từ vựng.");
        return ApiResponse<WordDto>.Ok(_mapper.Map<WordDto>(word));
    }

    public async Task<ApiResponse<WordDto>> CreateWordAsync(CreateWordRequest request)
    {
        var word = _mapper.Map<Word>(request);
        await _wordRepo.AddAsync(word);
        await _wordRepo.SaveChangesAsync();
        return ApiResponse<WordDto>.Ok(_mapper.Map<WordDto>(word), "Thêm từ vựng thành công.");
    }

    public async Task<ApiResponse<WordDto>> UpdateWordAsync(Guid id, UpdateWordRequest request)
    {
        var word = await _wordRepo.GetByIdAsync(id);
        if (word == null) return ApiResponse<WordDto>.Fail("Không tìm thấy từ vựng.");
        _mapper.Map(request, word);
        word.UpdatedAt = DateTime.UtcNow;
        _wordRepo.Update(word);
        await _wordRepo.SaveChangesAsync();
        return ApiResponse<WordDto>.Ok(_mapper.Map<WordDto>(word), "Cập nhật thành công.");
    }

    public async Task<ApiResponse<bool>> DeleteWordAsync(Guid id)
    {
        var word = await _wordRepo.GetByIdAsync(id);
        if (word == null) return ApiResponse<bool>.Fail("Không tìm thấy từ vựng.");
        _wordRepo.Remove(word);
        await _wordRepo.SaveChangesAsync();
        return ApiResponse<bool>.Ok(true, "Đã xóa từ vựng.");
    }

    // ── Favorites ────────────────────────────────────────────────────────────
    public async Task<ApiResponse<IEnumerable<WordDto>>> GetFavoritesAsync(Guid userId)
    {
        var favorites = await _favoriteRepo.GetByUserIdAsync(userId);
        var words = favorites.Select(f => f.Word!);
        return ApiResponse<IEnumerable<WordDto>>.Ok(_mapper.Map<List<WordDto>>(words));
    }

    public async Task<ApiResponse<bool>> AddFavoriteAsync(Guid userId, Guid wordId)
    {
        var exists = await _favoriteRepo.GetByUserAndWordAsync(userId, wordId);
        if (exists != null) return ApiResponse<bool>.Fail("Từ này đã có trong danh sách yêu thích.");

        await _favoriteRepo.AddAsync(new Favorite { UserId = userId, WordId = wordId });
        await _favoriteRepo.SaveChangesAsync();
        return ApiResponse<bool>.Ok(true, "Đã thêm vào yêu thích.");
    }

    public async Task<ApiResponse<bool>> RemoveFavoriteAsync(Guid userId, Guid wordId)
    {
        var fav = await _favoriteRepo.GetByUserAndWordAsync(userId, wordId);
        if (fav == null) return ApiResponse<bool>.Fail("Không tìm thấy.");
        _favoriteRepo.Remove(fav);
        await _favoriteRepo.SaveChangesAsync();
        return ApiResponse<bool>.Ok(true, "Đã xóa khỏi yêu thích.");
    }

    // ── Flashcards ───────────────────────────────────────────────────────────
    public async Task<ApiResponse<IEnumerable<FlashcardDto>>> GetFlashcardsAsync(Guid userId)
    {
        var cards = await _flashcardRepo.GetByUserIdAsync(userId);
        return ApiResponse<IEnumerable<FlashcardDto>>.Ok(_mapper.Map<List<FlashcardDto>>(cards));
    }

    public async Task<ApiResponse<FlashcardDto>> AddToFlashcardAsync(Guid userId, Guid wordId)
    {
        var exists = await _flashcardRepo.GetByUserAndWordAsync(userId, wordId);
        if (exists != null) return ApiResponse<FlashcardDto>.Fail("Từ này đã có trong flashcard.");

        var card = new Flashcard { UserId = userId, WordId = wordId };
        await _flashcardRepo.AddAsync(card);
        await _flashcardRepo.SaveChangesAsync();
        return ApiResponse<FlashcardDto>.Ok(_mapper.Map<FlashcardDto>(card), "Đã thêm vào flashcard.");
    }

    public async Task<ApiResponse<bool>> MarkFlashcardLearnedAsync(Guid userId, Guid wordId)
    {
        var card = await _flashcardRepo.GetByUserAndWordAsync(userId, wordId);
        if (card == null) return ApiResponse<bool>.Fail("Không tìm thấy flashcard.");
        card.IsLearned = true;
        card.LearnedAt = DateTime.UtcNow;
        _flashcardRepo.Update(card);
        await _flashcardRepo.SaveChangesAsync();
        return ApiResponse<bool>.Ok(true, "Đã đánh dấu học xong.");
    }

    public async Task<ApiResponse<bool>> RemoveFlashcardAsync(Guid userId, Guid wordId)
    {
        var card = await _flashcardRepo.GetByUserAndWordAsync(userId, wordId);
        if (card == null) return ApiResponse<bool>.Fail("Không tìm thấy flashcard.");
        _flashcardRepo.Remove(card);
        await _flashcardRepo.SaveChangesAsync();
        return ApiResponse<bool>.Ok(true, "Đã xóa flashcard.");
    }
}
