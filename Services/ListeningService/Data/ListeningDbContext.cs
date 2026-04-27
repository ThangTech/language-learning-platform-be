using ListeningService.Models;
using Microsoft.EntityFrameworkCore;

namespace ListeningService.Data;

/// <summary>
/// EF Core DbContext cho ListeningService.
/// Cấu hình Fluent API đầy đủ cho tất cả entity.
/// </summary>
public class ListeningDbContext : DbContext
{
    public ListeningDbContext(DbContextOptions<ListeningDbContext> options) : base(options) { }

    public DbSet<ListeningLesson> ListeningLessons => Set<ListeningLesson>();
    public DbSet<ListeningResult> ListeningResults => Set<ListeningResult>();
    public DbSet<DictationResult> DictationResults => Set<DictationResult>();
    public DbSet<Quiz> Quizzes => Set<Quiz>();
    public DbSet<QuizList> QuizLists => Set<QuizList>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── ListeningLesson ──────────────────────────────────────────────────
        modelBuilder.Entity<ListeningLesson>(e =>
        {
            e.ToTable("ListeningLessons");

            e.HasKey(x => x.Id);
            e.Property(x => x.Id)
             .HasDefaultValueSql("NEWID()");

            e.Property(x => x.Title)
             .IsRequired()
             .HasMaxLength(300);

            e.Property(x => x.AudioUrl)
             .IsRequired()
             .HasMaxLength(1000);

            e.Property(x => x.Transcript)
             .HasColumnType("nvarchar(max)");

            e.Property(x => x.Level)
             .HasMaxLength(20);

            e.Property(x => x.Part)
             .IsRequired();

            e.Property(x => x.Duration);

            e.Property(x => x.CreatedAt)
             .IsRequired()
             .HasDefaultValueSql("GETUTCDATE()");

            e.Property(x => x.UpdatedAt);

            // Index
            e.HasIndex(x => x.Part).HasDatabaseName("IX_ListeningLessons_Part");
            e.HasIndex(x => x.Level).HasDatabaseName("IX_ListeningLessons_Level");
            e.HasIndex(x => new { x.Part, x.Level }).HasDatabaseName("IX_ListeningLessons_Part_Level");
        });

        // ── ListeningResult ──────────────────────────────────────────────────
        modelBuilder.Entity<ListeningResult>(e =>
        {
            e.ToTable("ListeningResults");

            e.HasKey(x => x.Id);
            e.Property(x => x.Id)
             .HasDefaultValueSql("NEWID()");

            e.Property(x => x.UserId).IsRequired();
            e.Property(x => x.Score).IsRequired();
            e.Property(x => x.TimeTaken).IsRequired();
            e.Property(x => x.ListenCount).IsRequired();
            e.Property(x => x.CompletedAt)
             .IsRequired()
             .HasDefaultValueSql("GETUTCDATE()");

            // FK → ListeningLesson (Cascade delete)
            e.HasOne(x => x.Lesson)
             .WithMany(l => l.Results)
             .HasForeignKey(x => x.LessonId)
             .OnDelete(DeleteBehavior.Cascade);

            // Index
            e.HasIndex(x => x.UserId).HasDatabaseName("IX_ListeningResults_UserId");
            e.HasIndex(x => x.LessonId).HasDatabaseName("IX_ListeningResults_LessonId");
            e.HasIndex(x => new { x.UserId, x.LessonId }).HasDatabaseName("IX_ListeningResults_UserId_LessonId");
        });

        // ── DictationResult ──────────────────────────────────────────────────
        modelBuilder.Entity<DictationResult>(e =>
        {
            e.ToTable("DictationResults");

            e.HasKey(x => x.Id);
            e.Property(x => x.Id)
             .HasDefaultValueSql("NEWID()");

            e.Property(x => x.UserId).IsRequired();
            e.Property(x => x.UserTranscript)
             .IsRequired()
             .HasColumnType("nvarchar(max)");
            e.Property(x => x.Accuracy).IsRequired();
            e.Property(x => x.CompletedAt)
             .IsRequired()
             .HasDefaultValueSql("GETUTCDATE()");

            // FK → ListeningLesson (Cascade delete)
            e.HasOne(x => x.Lesson)
             .WithMany(l => l.DictationResults)
             .HasForeignKey(x => x.LessonId)
             .OnDelete(DeleteBehavior.Cascade);

            // Index
            e.HasIndex(x => x.UserId).HasDatabaseName("IX_DictationResults_UserId");
            e.HasIndex(x => x.LessonId).HasDatabaseName("IX_DictationResults_LessonId");
            e.HasIndex(x => new { x.UserId, x.LessonId }).HasDatabaseName("IX_DictationResults_UserId_LessonId");
        });

        // ── Quiz ─────────────────────────────────────────────────────────────
        modelBuilder.Entity<Quiz>(e =>
        {
            e.ToTable("Quizzes");

            e.HasKey(x => x.Id);
            e.Property(x => x.Id)
             .HasDefaultValueSql("NEWID()");

            e.Property(x => x.Title)
             .IsRequired()
             .HasMaxLength(300);

            // FK nullable → ListeningLesson (SetNull khi lesson bị xoá)
            e.HasOne(x => x.Lesson)
             .WithMany(l => l.Quizzes)
             .HasForeignKey(x => x.LessonId)
             .IsRequired(false)
             .OnDelete(DeleteBehavior.SetNull);

            e.HasIndex(x => x.LessonId).HasDatabaseName("IX_Quizzes_LessonId");
        });

        // ── QuizList ─────────────────────────────────────────────────────────
        modelBuilder.Entity<QuizList>(e =>
        {
            e.ToTable("QuizLists");

            e.HasKey(x => x.Id);
            e.Property(x => x.Id)
             .HasDefaultValueSql("NEWID()");

            e.Property(x => x.QuestionText)
             .IsRequired()
             .HasColumnType("nvarchar(max)");

            e.Property(x => x.QuestionType)
             .IsRequired()
             .HasMaxLength(30)
             .HasDefaultValue("MULTIPLE_CHOICE");

            e.Property(x => x.BlankText)
             .HasColumnType("nvarchar(max)");

            e.Property(x => x.ExpectedAnswer)
             .HasMaxLength(500);

            // FK → Quiz (Cascade delete)
            e.HasOne(x => x.Quiz)
             .WithMany(q => q.Questions)
             .HasForeignKey(x => x.QuizId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasIndex(x => x.QuizId).HasDatabaseName("IX_QuizLists_QuizId");
            e.HasIndex(x => x.QuestionType).HasDatabaseName("IX_QuizLists_QuestionType");
        });
    }
}
