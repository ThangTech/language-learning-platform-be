using AutoMapper;
using LanguagePlatform.Application.DTOs.Common;
using LanguagePlatform.Application.DTOs.Listening;
using LanguagePlatform.Application.Interfaces;
using LanguagePlatform.Domain.Entities;
using LanguagePlatform.Domain.Interfaces;

namespace LanguagePlatform.Application.Services;

public class ListeningService : IListeningService
{
    private readonly IListeningRepository _lessonRepo;
    private readonly IListeningResultRepository _resultRepo;
    private readonly IDictationSetRepository _dictationRepo;
    private readonly IMapper _mapper;

    public ListeningService(
        IListeningRepository lessonRepo,
        IListeningResultRepository resultRepo,
        IDictationSetRepository dictationRepo,
        IMapper mapper)
    {
        _lessonRepo = lessonRepo;
        _resultRepo = resultRepo;
        _dictationRepo = dictationRepo;
        _mapper = mapper;
    }

    public async Task<ApiResponse<PagedResult<ListeningLessonDto>>> GetLessonsAsync(
        int page, int pageSize, string? level = null, string? search = null)
    {
        var (items, total) = await _lessonRepo.GetLessonsPagedAsync(page, pageSize, level, search);
        return ApiResponse<PagedResult<ListeningLessonDto>>.Ok(new PagedResult<ListeningLessonDto>
        {
            Items = _mapper.Map<List<ListeningLessonDto>>(items),
            TotalCount = total,
            Page = page,
            PageSize = pageSize
        });
    }

    public async Task<ApiResponse<ListeningLessonDto>> GetLessonByIdAsync(Guid id)
    {
        var lesson = await _lessonRepo.GetByIdAsync(id);
        if (lesson == null) return ApiResponse<ListeningLessonDto>.Fail("Không tìm thấy bài nghe.");
        return ApiResponse<ListeningLessonDto>.Ok(_mapper.Map<ListeningLessonDto>(lesson));
    }

    public async Task<ApiResponse<ListeningLessonDto>> CreateLessonAsync(CreateListeningLessonRequest request)
    {
        var lesson = _mapper.Map<ListeningLesson>(request);
        await _lessonRepo.AddAsync(lesson);
        await _lessonRepo.SaveChangesAsync();
        return ApiResponse<ListeningLessonDto>.Ok(_mapper.Map<ListeningLessonDto>(lesson), "Đã tạo bài nghe.");
    }

    public async Task<ApiResponse<ListeningLessonDto>> UpdateLessonAsync(Guid id, UpdateListeningLessonRequest request)
    {
        var lesson = await _lessonRepo.GetByIdAsync(id);
        if (lesson == null) return ApiResponse<ListeningLessonDto>.Fail("Không tìm thấy bài nghe.");
        _mapper.Map(request, lesson);
        lesson.UpdatedAt = DateTime.UtcNow;
        _lessonRepo.Update(lesson);
        await _lessonRepo.SaveChangesAsync();
        return ApiResponse<ListeningLessonDto>.Ok(_mapper.Map<ListeningLessonDto>(lesson), "Cập nhật thành công.");
    }

    public async Task<ApiResponse<bool>> DeleteLessonAsync(Guid id)
    {
        var lesson = await _lessonRepo.GetByIdAsync(id);
        if (lesson == null) return ApiResponse<bool>.Fail("Không tìm thấy bài nghe.");
        _lessonRepo.Remove(lesson);
        await _lessonRepo.SaveChangesAsync();
        return ApiResponse<bool>.Ok(true, "Đã xóa bài nghe.");
    }

    public async Task<ApiResponse<ListeningResultDto>> SubmitResultAsync(Guid userId, SubmitListeningResultRequest request)
    {
        var result = new ListeningResult
        {
            UserId = userId,
            LessonId = request.LessonId,
            Score = request.Score,
            CompletedAt = DateTime.UtcNow
        };
        await _resultRepo.AddAsync(result);
        await _resultRepo.SaveChangesAsync();
        return ApiResponse<ListeningResultDto>.Ok(_mapper.Map<ListeningResultDto>(result), "Đã lưu kết quả.");
    }

    public async Task<ApiResponse<IEnumerable<ListeningResultDto>>> GetUserResultsAsync(Guid userId)
    {
        var results = await _resultRepo.GetByUserIdAsync(userId);
        return ApiResponse<IEnumerable<ListeningResultDto>>.Ok(_mapper.Map<List<ListeningResultDto>>(results));
    }

    public async Task<ApiResponse<IEnumerable<DictationSetDto>>> GetDictationSetsAsync()
    {
        var sets = await _dictationRepo.GetAllWithSentencesAsync();
        return ApiResponse<IEnumerable<DictationSetDto>>.Ok(_mapper.Map<List<DictationSetDto>>(sets));
    }

    public async Task<ApiResponse<DictationSetDto>> GetDictationSetByIdAsync(Guid id)
    {
        var set = await _dictationRepo.GetWithSentencesAsync(id);
        if (set == null) return ApiResponse<DictationSetDto>.Fail("Không tìm thấy bộ chính tả.");
        return ApiResponse<DictationSetDto>.Ok(_mapper.Map<DictationSetDto>(set));
    }

    public async Task<ApiResponse<DictationSetDto>> CreateDictationSetAsync(CreateDictationSetRequest request)
    {
        var set = new DictationSet
        {
            Title = request.Title,
            Level = request.Level,
            Sentences = request.Sentences.Select(s => new DictationSentence
            {
                Sentence = s.Sentence,
                AudioUrl = s.AudioUrl,
                OrderIndex = s.OrderIndex
            }).ToList()
        };
        await _dictationRepo.AddAsync(set);
        await _dictationRepo.SaveChangesAsync();
        return ApiResponse<DictationSetDto>.Ok(_mapper.Map<DictationSetDto>(set), "Đã tạo bộ chính tả.");
    }
}
