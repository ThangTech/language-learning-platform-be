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
    private readonly IMapper _mapper;

    public QuizService(
        IQuizRepository quizRepo,
        IMapper mapper)
    {
        _quizRepo = quizRepo;
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

        return ApiResponse<QuizResultDto>.Ok(
            result,
            "Nộp bài thành công.");
    }

    private static QuizDifficulty ParseDifficulty(string value)
    {
        return Enum.Parse<QuizDifficulty>(value, true);
    }

    private static QuizType ParseQuizType(string value)
    {
        return Enum.Parse<QuizType>(value, true);
    }

    private static QuestionType ParseQuestionType(string value)
    {
        return Enum.Parse<QuestionType>(value, true);
    }
}
