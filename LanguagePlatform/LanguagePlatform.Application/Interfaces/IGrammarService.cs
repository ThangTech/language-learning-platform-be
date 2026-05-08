using LanguagePlatform.Application.DTOs.Common;
using LanguagePlatform.Application.DTOs.Grammar;

namespace LanguagePlatform.Application.Interfaces;

public interface IGrammarService
{
    Task<ApiResponse<PagedResult<GrammarTopicDto>>> GetTopicsAsync(int page, int pageSize, string? level = null, string? search = null);
    Task<ApiResponse<GrammarTopicDto>> GetTopicByIdAsync(Guid id);
    Task<ApiResponse<GrammarTopicDto>> CreateTopicAsync(CreateGrammarTopicRequest request);
    Task<ApiResponse<GrammarTopicDto>> UpdateTopicAsync(Guid id, UpdateGrammarTopicRequest request);
    Task<ApiResponse<bool>> DeleteTopicAsync(Guid id);

    // User grammar progress
    Task<ApiResponse<IEnumerable<UserGrammarDto>>> GetUserGrammarProgressAsync(Guid userId);
    Task<ApiResponse<bool>> MarkTopicCompletedAsync(Guid userId, Guid topicId);
}
