using Microsoft.EntityFrameworkCore;
using QuizService.Models;
namespace QuizService.Data;
public class QuizDbContext : DbContext
{
    public QuizDbContext(DbContextOptions<QuizDbContext> options) : base(options) { }
    public DbSet<Quiz> Quizzes => Set<Quiz>();
    protected override void OnModelCreating(ModelBuilder modelBuilder) { base.OnModelCreating(modelBuilder); }
}
