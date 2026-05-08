using LanguagePlatform.Domain.Entities;
using LanguagePlatform.Domain.Interfaces;
using LanguagePlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LanguagePlatform.Infrastructure.Repositories;

public class QuizRepository : GenericRepository<Quiz>, IQuizRepository
{
    public QuizRepository(AppDbContext context) : base(context) { }

    public async Task<Quiz?> GetWithQuestionsAsync(Guid id)
        => await _dbSet.Include(q => q.Questions).FirstOrDefaultAsync(q => q.Id == id);

    public async Task<IEnumerable<Quiz>> GetByLessonAsync(Guid lessonId)
        => await _dbSet.Include(q => q.Questions).Where(q => q.LessonId == lessonId).ToListAsync();
}
