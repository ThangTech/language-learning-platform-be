using LanguagePlatform.Application.DTOs.Common;
using LanguagePlatform.Application.DTOs.Listening;

namespace LanguagePlatform.Application.Interfaces;

public interface IListeningService
{
    // Lessons
    Task<ApiResponse<PagedResult<ListeningLessonDto>>> GetLessonsAsync(int page, int pageSize, string? level = null, string? search = null);
    Task<ApiResponse<ListeningLessonDto>> GetLessonByIdAsync(Guid id);
    Task<ApiResponse<ListeningLessonDto>> CreateLessonAsync(CreateListeningLessonRequest request);
    Task<ApiResponse<ListeningLessonDto>> UpdateLessonAsync(Guid id, UpdateListeningLessonRequest request);
    Task<ApiResponse<bool>> DeleteLessonAsync(Guid id);

    // Results
    Task<ApiResponse<ListeningResultDto>> SubmitResultAsync(Guid userId, SubmitListeningResultRequest request);
    Task<ApiResponse<IEnumerable<ListeningResultDto>>> GetUserResultsAsync(Guid userId);

    // Dictation
    Task<ApiResponse<IEnumerable<DictationSetDto>>> GetDictationSetsAsync();
    Task<ApiResponse<DictationSetDto>> GetDictationSetByIdAsync(Guid id);
    Task<ApiResponse<DictationSetDto>> CreateDictationSetAsync(CreateDictationSetRequest request);
}
