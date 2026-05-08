using LanguagePlatform.Application.DTOs.Common;
using LanguagePlatform.Application.DTOs.Quiz;

namespace LanguagePlatform.Application.Interfaces;

public interface IQuizService
{
    Task<ApiResponse<IEnumerable<QuizDto>>> GetQuizzesAsync();
    Task<ApiResponse<QuizDto>> GetQuizByIdAsync(Guid id);
    Task<ApiResponse<IEnumerable<QuizDto>>> GetQuizzesByLessonAsync(Guid lessonId);
    Task<ApiResponse<QuizDto>> CreateQuizAsync(CreateQuizRequest request);
    Task<ApiResponse<QuizDto>> UpdateQuizAsync(Guid id, UpdateQuizRequest request);
    Task<ApiResponse<bool>> DeleteQuizAsync(Guid id);
    Task<ApiResponse<QuizResultDto>> SubmitQuizAsync(Guid userId, SubmitQuizRequest request);
}
