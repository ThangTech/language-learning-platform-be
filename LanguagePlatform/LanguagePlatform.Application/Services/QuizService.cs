using AutoMapper;
using LanguagePlatform.Application.DTOs.Common;
using LanguagePlatform.Application.DTOs.Quiz;
using LanguagePlatform.Application.Interfaces;
using LanguagePlatform.Domain.Entities;
using LanguagePlatform.Domain.Enums;
using LanguagePlatform.Domain.Interfaces;

namespace LanguagePlatform.Application.Services;

public class QuizService : IQuizService
{
    private readonly IQuizRepository _quizRepo;
    private readonly IQuizResultRepository _quizResultRepo;
    private readonly IProgressService _progressService;
    private readonly IMapper _mapper;

    public QuizService(
        IQuizRepository quizRepo,
        IQuizResultRepository quizResultRepo,
        IProgressService progressService,
        IMapper mapper)
    {
        _quizRepo = quizRepo;
        _quizResultRepo = quizResultRepo;
        _progressService = progressService;
        _mapper = mapper;
    }

    public async Task<ApiResponse<IEnumerable<QuizDto>>> GetQuizzesAsync()
    {
        var quizzes = await _quizRepo.GetAllAsync();
        var quizDtos = _mapper.Map<List<QuizDto>>(quizzes);

        return ApiResponse<IEnumerable<QuizDto>>.Ok(quizDtos);
    }

    public async Task<ApiResponse<QuizDto>> GetQuizByIdAsync(Guid id)
    {
        var quiz = await _quizRepo.GetWithQuestionsAsync(id);

        if (quiz == null)
        {
            return ApiResponse<QuizDto>.Fail("Không tìm thấy bài quiz.");
        }

        var quizDto = _mapper.Map<QuizDto>(quiz);

        return ApiResponse<QuizDto>.Ok(quizDto);
    }

    public async Task<ApiResponse<IEnumerable<QuizDto>>> GetQuizzesByLessonAsync(Guid lessonId)
    {
        var quizzes = await _quizRepo.GetByLessonAsync(lessonId);
        var quizDtos = _mapper.Map<List<QuizDto>>(quizzes);

        return ApiResponse<IEnumerable<QuizDto>>.Ok(quizDtos);
    }

    public async Task<ApiResponse<IEnumerable<QuizDto>>> GetQuizzesByGrammarTopicAsync(Guid grammarTopicId)
    {
        var quizzes = await _quizRepo.GetByGrammarTopicAsync(grammarTopicId);
        var quizDtos = _mapper.Map<List<QuizDto>>(quizzes);

        return ApiResponse<IEnumerable<QuizDto>>.Ok(quizDtos);
    }

    public async Task<ApiResponse<QuizDto>> CreateQuizAsync(CreateQuizRequest request)
    {
        var questions = request.Questions
            .Select(question => new QuizQuestion
            {
                QuestionText = question.QuestionText,
                Type = ParseQuestionType(question.Type),
                Options = question.Options,
                CorrectAnswer = question.CorrectAnswer,
                Explanation = question.Explanation,
                AudioUrl = question.AudioUrl
            })
            .ToList();

        var quiz = new Quiz
        {
            Title = request.Title,
            LessonId = request.LessonId,
            GrammarTopicId = request.GrammarTopicId,
            Difficulty = ParseDifficulty(request.Difficulty),
            Type = ParseQuizType(request.Type),
            DurationMinutes = request.DurationMinutes,
            Questions = questions
        };

        await _quizRepo.AddAsync(quiz);
        await _quizRepo.SaveChangesAsync();

        var quizDto = _mapper.Map<QuizDto>(quiz);

        return ApiResponse<QuizDto>.Ok(
            quizDto,
            "Đã tạo quiz.");
    }

    public async Task<ApiResponse<QuizDto>> UpdateQuizAsync(
        Guid id,
        UpdateQuizRequest request)
    {
        var quiz = await _quizRepo.GetWithQuestionsAsync(id);

        if (quiz == null)
        {
            return ApiResponse<QuizDto>.Fail("Không tìm thấy quiz.");
        }

        quiz.Title = request.Title;
        quiz.LessonId = request.LessonId;
        quiz.GrammarTopicId = request.GrammarTopicId;
        quiz.Difficulty = ParseDifficulty(request.Difficulty);
        quiz.Type = ParseQuizType(request.Type);
        quiz.DurationMinutes = request.DurationMinutes;

        _quizRepo.Update(quiz);
        await _quizRepo.SaveChangesAsync();

        var quizDto = _mapper.Map<QuizDto>(quiz);

        return ApiResponse<QuizDto>.Ok(
            quizDto,
            "Cập nhật thành công.");
    }

    public async Task<ApiResponse<bool>> DeleteQuizAsync(Guid id)
    {
        var quiz = await _quizRepo.GetByIdAsync(id);

        if (quiz == null)
        {
            return ApiResponse<bool>.Fail("Không tìm thấy quiz.");
        }

        _quizRepo.Delete(quiz);
        await _quizRepo.SaveChangesAsync();

        return ApiResponse<bool>.Ok(
            true,
            "Đã xóa quiz.");
    }

    public async Task<ApiResponse<QuizResultDto>> SubmitQuizAsync(
        Guid userId,
        SubmitQuizRequest request)
    {
        var quiz = await _quizRepo.GetWithQuestionsAsync(request.QuizId);

        if (quiz == null)
        {
            return ApiResponse<QuizResultDto>.Fail("Không tìm thấy quiz.");
        }

        var correct = 0;
        var details = new List<QuizAnswerResultDto>();

        foreach (var answer in request.Answers)
        {
            var question = quiz.Questions
                .FirstOrDefault(q => q.Id == answer.QuestionId);

            if (question == null)
            {
                continue;
            }

            var isCorrect = string.Equals(
                answer.Answer,
                question.CorrectAnswer,
                StringComparison.OrdinalIgnoreCase);

            if (isCorrect)
            {
                correct++;
            }

            details.Add(new QuizAnswerResultDto
            {
                QuestionId = answer.QuestionId,
                IsCorrect = isCorrect,
                CorrectAnswer = question.CorrectAnswer,
                Explanation = question.Explanation
            });
        }

        var total = quiz.Questions.Count;
        var score = total > 0
            ? (int)Math.Round((double)correct / total * 100)
            : 0;

        var result = new QuizResultDto
        {
            QuizId = quiz.Id,
            Score = score,
            CorrectAnswers = correct,
            TotalQuestions = total,
            Answers = details
        };

        await _progressService.RecordCompletionAsync(userId, "quiz", score);

        // Lưu kết quả vào database
        var quizResult = new QuizResult
        {
            UserId = userId,
            QuizId = quiz.Id,
            Score = score,
            TotalQuestions = total,
            CorrectAnswers = correct,
            CompletedAt = DateTime.UtcNow
        };
        await _quizResultRepo.AddAsync(quizResult);
        await _quizResultRepo.SaveChangesAsync();

        return ApiResponse<QuizResultDto>.Ok(
            result,
            "Nộp bài thành công.");
    }

    public async Task<ApiResponse<IEnumerable<QuizHistoryDto>>> GetMyResultsAsync(Guid userId)
    {
        var results = await _quizResultRepo.GetByUserIdAsync(userId);
        var dtos = results.Select(r => new QuizHistoryDto
        {
            Id = r.Id,
            QuizId = r.QuizId,
            QuizTitle = r.Quiz?.Title ?? "Quiz",
            Score = r.Score,
            TotalQuestions = r.TotalQuestions,
            CorrectAnswers = r.CorrectAnswers,
            CompletedAt = r.CompletedAt
        });
        return ApiResponse<IEnumerable<QuizHistoryDto>>.Ok(dtos);
    }

    private static QuizDifficulty ParseDifficulty(string value)
    {
        return Enum.TryParse<QuizDifficulty>(value, true, out var result) ? result : QuizDifficulty.Easy;
    }

    private static QuizType ParseQuizType(string value)
    {
        return Enum.TryParse<QuizType>(value, true, out var result) ? result : QuizType.MultipleChoice;
    }

    private static QuestionType ParseQuestionType(string value)
    {
        return Enum.TryParse<QuestionType>(value, true, out var result) ? result : QuestionType.MultipleChoice;
    }
}
