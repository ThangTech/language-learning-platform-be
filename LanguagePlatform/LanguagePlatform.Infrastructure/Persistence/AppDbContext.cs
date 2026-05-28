using LanguagePlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LanguagePlatform.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Word> Words => Set<Word>();
    public DbSet<Favorite> Favorites => Set<Favorite>();
    public DbSet<Flashcard> Flashcards => Set<Flashcard>();
    public DbSet<GrammarTopic> GrammarTopics => Set<GrammarTopic>();
    public DbSet<UserGrammar> UserGrammars => Set<UserGrammar>();
    public DbSet<ListeningLesson> ListeningLessons => Set<ListeningLesson>();
    public DbSet<ListeningResult> ListeningResults => Set<ListeningResult>();
    public DbSet<DictationSet> DictationSets => Set<DictationSet>();
    public DbSet<DictationSentence> DictationSentences => Set<DictationSentence>();
    public DbSet<Quiz> Quizzes => Set<Quiz>();
    public DbSet<QuizQuestion> QuizQuestions => Set<QuizQuestion>();
    public DbSet<QuizResult> QuizResults => Set<QuizResult>();
    public DbSet<UserProgress> UserProgresses => Set<UserProgress>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Email).IsUnique();
            e.Property(x => x.Email).IsRequired().HasMaxLength(256);
            e.Property(x => x.FullName).IsRequired().HasMaxLength(100);
            e.Property(x => x.PasswordHash).IsRequired();
        });

        // Word
        modelBuilder.Entity<Word>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Term).IsRequired().HasMaxLength(200);
            e.Property(x => x.Definition).IsRequired();
        });

        // Favorite
        modelBuilder.Entity<Favorite>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => new { x.UserId, x.WordId }).IsUnique();
            e.HasOne(x => x.User).WithMany(u => u.Favorites).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Word).WithMany().HasForeignKey(x => x.WordId).OnDelete(DeleteBehavior.NoAction);
        });

        // Flashcard
        modelBuilder.Entity<Flashcard>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => new { x.UserId, x.WordId }).IsUnique();
            e.HasOne(x => x.User).WithMany(u => u.Flashcards).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Word).WithMany().HasForeignKey(x => x.WordId).OnDelete(DeleteBehavior.NoAction);
        });

        // GrammarTopic
        modelBuilder.Entity<GrammarTopic>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Title).IsRequired().HasMaxLength(200);
        });

        // UserGrammar
        modelBuilder.Entity<UserGrammar>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => new { x.UserId, x.TopicId }).IsUnique();
            e.HasOne(x => x.User).WithMany(u => u.UserGrammars).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Topic).WithMany(t => t.UserGrammars).HasForeignKey(x => x.TopicId).OnDelete(DeleteBehavior.Cascade);
        });

        // ListeningLesson
        modelBuilder.Entity<ListeningLesson>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Title).IsRequired().HasMaxLength(200);
        });

        // ListeningResult
        modelBuilder.Entity<ListeningResult>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Lesson).WithMany(l => l.Results).HasForeignKey(x => x.LessonId).OnDelete(DeleteBehavior.Cascade);
        });

        // DictationSet
        modelBuilder.Entity<DictationSet>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasMany(x => x.Sentences).WithOne(s => s.DictationSet).HasForeignKey(s => s.DictationSetId).OnDelete(DeleteBehavior.Cascade);
        });

        // DictationSentence
        modelBuilder.Entity<DictationSentence>(e =>
        {
            e.HasKey(x => x.Id);
        });

        // Quiz
        modelBuilder.Entity<Quiz>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasMany(x => x.Questions).WithOne(q => q.Quiz).HasForeignKey(q => q.QuizId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Lesson).WithMany(l => l.Quizzes).HasForeignKey(x => x.LessonId).OnDelete(DeleteBehavior.SetNull);
        });

        // QuizResult
        modelBuilder.Entity<QuizResult>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Quiz).WithMany(q => q.Results).HasForeignKey(x => x.QuizId).OnDelete(DeleteBehavior.Cascade);
        });

        // QuizQuestion
        modelBuilder.Entity<QuizQuestion>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Options).HasConversion(
                v => string.Join("||", v),
                v => v.Split("||", StringSplitOptions.RemoveEmptyEntries).ToList()
            );
        });

        // UserProgress
        modelBuilder.Entity<UserProgress>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.UserId).IsUnique();
            e.HasOne(x => x.User).WithOne(u => u.Progress).HasForeignKey<UserProgress>(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        });

        // Notification
        modelBuilder.Entity<Notification>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasOne(x => x.User).WithMany(u => u.Notifications).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        });
    }
}
