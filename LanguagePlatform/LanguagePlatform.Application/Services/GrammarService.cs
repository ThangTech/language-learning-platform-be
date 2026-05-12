using AutoMapper;
using LanguagePlatform.Application.DTOs.Common;
using LanguagePlatform.Application.DTOs.Grammar;
using LanguagePlatform.Application.Interfaces;
using LanguagePlatform.Domain.Entities;
using LanguagePlatform.Domain.Enums;
using LanguagePlatform.Domain.Interfaces;

namespace LanguagePlatform.Application.Services;

public class GrammarService : IGrammarService
{
    private readonly IGrammarRepository _grammarRepo;
    private readonly IUserGrammarRepository _userGrammarRepo;
    private readonly IProgressService _progressService;
    private readonly IMapper _mapper;

    public GrammarService(
        IGrammarRepository grammarRepo,
        IUserGrammarRepository userGrammarRepo,
        IProgressService progressService,
        IMapper mapper)
    {
        _grammarRepo = grammarRepo;
        _userGrammarRepo = userGrammarRepo;
        _progressService = progressService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<PagedResult<GrammarTopicDto>>> GetTopicsAsync(
        int page, int pageSize, string? level = null, string? search = null)
    {
        GrammarLevel? lvl = null;
        if (!string.IsNullOrWhiteSpace(level) && Enum.TryParse<GrammarLevel>(level, true, out var parsed))
            lvl = parsed;

        var (items, total) = await _grammarRepo.GetTopicsPagedAsync(page, pageSize, lvl, search);
        return ApiResponse<PagedResult<GrammarTopicDto>>.Ok(new PagedResult<GrammarTopicDto>
        {
            Items = _mapper.Map<List<GrammarTopicDto>>(items),
            TotalCount = total,
            Page = page,
            PageSize = pageSize
        });
    }

    public async Task<ApiResponse<GrammarTopicDto>> GetTopicByIdAsync(Guid id)
    {
        var topic = await _grammarRepo.GetByIdAsync(id);
        if (topic == null) return ApiResponse<GrammarTopicDto>.Fail("Không tìm thấy chủ đề ngữ pháp.");
        return ApiResponse<GrammarTopicDto>.Ok(_mapper.Map<GrammarTopicDto>(topic));
    }

    public async Task<ApiResponse<GrammarTopicDto>> CreateTopicAsync(CreateGrammarTopicRequest request)
    {
        var topic = _mapper.Map<GrammarTopic>(request);
        await _grammarRepo.AddAsync(topic);
        await _grammarRepo.SaveChangesAsync();
        return ApiResponse<GrammarTopicDto>.Ok(_mapper.Map<GrammarTopicDto>(topic), "Đã tạo chủ đề.");
    }

    public async Task<ApiResponse<GrammarTopicDto>> UpdateTopicAsync(Guid id, UpdateGrammarTopicRequest request)
    {
        var topic = await _grammarRepo.GetByIdAsync(id);
        if (topic == null) return ApiResponse<GrammarTopicDto>.Fail("Không tìm thấy chủ đề.");
        _mapper.Map(request, topic);
        topic.UpdatedAt = DateTime.UtcNow;
        _grammarRepo.Update(topic);
        await _grammarRepo.SaveChangesAsync();
        return ApiResponse<GrammarTopicDto>.Ok(_mapper.Map<GrammarTopicDto>(topic), "Cập nhật thành công.");
    }

    public async Task<ApiResponse<bool>> DeleteTopicAsync(Guid id)
    {
        var topic = await _grammarRepo.GetByIdAsync(id);
        if (topic == null) return ApiResponse<bool>.Fail("Không tìm thấy chủ đề.");
        _grammarRepo.Delete(topic);
        await _grammarRepo.SaveChangesAsync();
        return ApiResponse<bool>.Ok(true, "Đã xóa chủ đề.");
    }

    public async Task<ApiResponse<IEnumerable<UserGrammarDto>>> GetUserGrammarProgressAsync(Guid userId)
    {
        var items = await _userGrammarRepo.GetByUserIdAsync(userId);
        return ApiResponse<IEnumerable<UserGrammarDto>>.Ok(_mapper.Map<List<UserGrammarDto>>(items));
    }

    public async Task<ApiResponse<bool>> MarkTopicCompletedAsync(Guid userId, Guid topicId)
    {
        var existing = await _userGrammarRepo.GetByUserAndTopicAsync(userId, topicId);
        var isNewCompletion = false;

        if (existing != null)
        {
            if (!existing.IsCompleted)
            {
                existing.IsCompleted = true;
                existing.CompletedAt = DateTime.UtcNow;
                _userGrammarRepo.Update(existing);
                isNewCompletion = true;
            }
        }
        else
        {
            var userGrammar = new UserGrammar
            {
                UserId = userId,
                TopicId = topicId,
                IsCompleted = true,
                CompletedAt = DateTime.UtcNow
            };

            await _userGrammarRepo.AddAsync(userGrammar);
            isNewCompletion = true;
        }

        await _userGrammarRepo.SaveChangesAsync();

        if (isNewCompletion)
        {
            await _progressService.RecordCompletionAsync(userId, "grammar", 5);
        }

        return ApiResponse<bool>.Ok(true, "Đã đánh dấu hoàn thành.");
    }
}
