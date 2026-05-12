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
    private readonly IProgressService _progressService;
    private readonly IMapper _mapper;

    public ListeningService(
        IListeningRepository lessonRepo,
        IListeningResultRepository resultRepo,
        IDictationSetRepository dictationRepo,
        IProgressService progressService,
        IMapper mapper)
    {
        _lessonRepo = lessonRepo;
        _resultRepo = resultRepo;
        _dictationRepo = dictationRepo;
        _progressService = progressService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<PagedResult<ListeningLessonDto>>> GetLessonsAsync(
        int page,
        int pageSize,
        string? level = null,
        string? search = null)
    {
        var result = await _lessonRepo.GetLessonsPagedAsync(
            page,
            pageSize,
            level,
            search);

        var lessons = _mapper.Map<List<ListeningLessonDto>>(result.Items);

        var pagedResult = new PagedResult<ListeningLessonDto>
        {
            Items = lessons,
            TotalCount = result.TotalCount,
            Page = page,
            PageSize = pageSize
        };

        return ApiResponse<PagedResult<ListeningLessonDto>>.Ok(pagedResult);
    }

    public async Task<ApiResponse<ListeningLessonDto>> GetLessonByIdAsync(Guid id)
    {
        var lesson = await _lessonRepo.GetByIdAsync(id);

        if (lesson == null)
        {
            return ApiResponse<ListeningLessonDto>.Fail("Không tìm thấy bài nghe.");
        }

        var lessonDto = _mapper.Map<ListeningLessonDto>(lesson);

        return ApiResponse<ListeningLessonDto>.Ok(lessonDto);
    }

    public async Task<ApiResponse<ListeningLessonDto>> CreateLessonAsync(
        CreateListeningLessonRequest request)
    {
        var lesson = _mapper.Map<ListeningLesson>(request);

        await _lessonRepo.AddAsync(lesson);
        await _lessonRepo.SaveChangesAsync();

        var lessonDto = _mapper.Map<ListeningLessonDto>(lesson);

        return ApiResponse<ListeningLessonDto>.Ok(
            lessonDto,
            "Đã tạo bài nghe.");
    }

    public async Task<ApiResponse<ListeningLessonDto>> UpdateLessonAsync(
        Guid id,
        UpdateListeningLessonRequest request)
    {
        var lesson = await _lessonRepo.GetByIdAsync(id);

        if (lesson == null)
        {
            return ApiResponse<ListeningLessonDto>.Fail("Không tìm thấy bài nghe.");
        }

        _mapper.Map(
            request,
            lesson);

        lesson.UpdatedAt = DateTime.UtcNow;

        _lessonRepo.Update(lesson);
        await _lessonRepo.SaveChangesAsync();

        var lessonDto = _mapper.Map<ListeningLessonDto>(lesson);

        return ApiResponse<ListeningLessonDto>.Ok(
            lessonDto,
            "Cập nhật thành công.");
    }

    public async Task<ApiResponse<bool>> DeleteLessonAsync(Guid id)
    {
        var lesson = await _lessonRepo.GetByIdAsync(id);

        if (lesson == null)
        {
            return ApiResponse<bool>.Fail("Không tìm thấy bài nghe.");
        }

        _lessonRepo.Delete(lesson);
        await _lessonRepo.SaveChangesAsync();

        return ApiResponse<bool>.Ok(
            true,
            "Đã xóa bài nghe.");
    }

    public async Task<ApiResponse<ListeningResultDto>> SubmitResultAsync(
        Guid userId,
        SubmitListeningResultRequest request)
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

        await _progressService.RecordCompletionAsync(userId, "listening", request.Score);

        var resultDto = _mapper.Map<ListeningResultDto>(result);

        return ApiResponse<ListeningResultDto>.Ok(
            resultDto,
            "Đã lưu kết quả.");
    }

    public async Task<ApiResponse<IEnumerable<ListeningResultDto>>> GetUserResultsAsync(
        Guid userId)
    {
        var results = await _resultRepo.GetByUserIdAsync(userId);
        var resultDtos = _mapper.Map<List<ListeningResultDto>>(results);

        return ApiResponse<IEnumerable<ListeningResultDto>>.Ok(resultDtos);
    }

    public async Task<ApiResponse<IEnumerable<DictationSetDto>>> GetDictationSetsAsync()
    {
        var sets = await _dictationRepo.GetAllWithSentencesAsync();
        var setDtos = _mapper.Map<List<DictationSetDto>>(sets);

        return ApiResponse<IEnumerable<DictationSetDto>>.Ok(setDtos);
    }

    public async Task<ApiResponse<DictationSetDto>> GetDictationSetByIdAsync(Guid id)
    {
        var set = await _dictationRepo.GetWithSentencesAsync(id);

        if (set == null)
        {
            return ApiResponse<DictationSetDto>.Fail("Không tìm thấy bộ chính tả.");
        }

        var setDto = _mapper.Map<DictationSetDto>(set);

        return ApiResponse<DictationSetDto>.Ok(setDto);
    }

    public async Task<ApiResponse<DictationSetDto>> CreateDictationSetAsync(
        CreateDictationSetRequest request)
    {
        var sentences = request.Sentences
            .Select(sentence => new DictationSentence
            {
                Sentence = sentence.Sentence,
                AudioTitle = sentence.AudioTitle,
                AudioUrl = sentence.AudioUrl,
                Hint = sentence.Hint,
                Duration = sentence.Duration,
                OrderIndex = sentence.OrderIndex
            })
            .ToList();

        var set = new DictationSet
        {
            Title = request.Title,
            Description = request.Description,
            Level = request.Level,
            Topic = request.Topic,
            Sentences = sentences
        };

        await _dictationRepo.AddAsync(set);
        await _dictationRepo.SaveChangesAsync();

        var setDto = _mapper.Map<DictationSetDto>(set);

        return ApiResponse<DictationSetDto>.Ok(
            setDto,
            "Đã tạo bộ chính tả.");
    }
}
