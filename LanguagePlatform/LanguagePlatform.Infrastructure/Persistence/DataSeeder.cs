using LanguagePlatform.Domain.Entities;
using LanguagePlatform.Domain.Enums;
using LanguagePlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LanguagePlatform.Infrastructure.Persistence;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        // Chạy pending migrations trước
        await db.Database.MigrateAsync();

        await SeedAdminUserAsync(db);
        await SeedSampleWordsAsync(db);
        await SeedSampleGrammarAsync(db);
    }

    // ── Admin User ────────────────────────────────────────────────────────────

    private static async Task SeedAdminUserAsync(AppDbContext db)
    {
        if (await db.Users.AnyAsync(u => u.Role == UserRole.Admin))
            return;

        var admin = new User
        {
            Id = Guid.NewGuid(),
            Email = "admin@languageplatform.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123456"),
            FullName = "System Administrator",
            Role = UserRole.Admin,
            Status = UserStatus.Active,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        db.Users.Add(admin);
        await db.SaveChangesAsync();
    }

    // ── Sample Words ──────────────────────────────────────────────────────────

    private static async Task SeedSampleWordsAsync(AppDbContext db)
    {
        if (await db.Words.AnyAsync())
            return;

        var words = new List<Word>
        {
            new() { Id = Guid.NewGuid(), Term = "abandon", Definition = "to leave completely and finally", ExampleSentence = "She had to abandon her car in the snow.", Level = WordLevel.Intermediate, Topic = "General", ImageUrl = null, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Term = "ability", Definition = "the physical or mental power or skill needed to do something", ExampleSentence = "She has the ability to solve complex problems.", Level = WordLevel.Beginner, Topic = "General", ImageUrl = null, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Term = "abolish", Definition = "to officially end a law, system, or institution", ExampleSentence = "Slavery was abolished in the US in 1865.", Level = WordLevel.Advanced, Topic = "History", ImageUrl = null, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Term = "abstract", Definition = "existing as an idea, feeling, or quality, not as a material object", ExampleSentence = "Love is an abstract concept.", Level = WordLevel.Intermediate, Topic = "Academic", ImageUrl = null, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Term = "accelerate", Definition = "to move faster or make something happen faster", ExampleSentence = "The car accelerated down the highway.", Level = WordLevel.Intermediate, Topic = "Science", ImageUrl = null, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
        };

        db.Words.AddRange(words);
        await db.SaveChangesAsync();
    }

    // ── Sample Grammar Topics ─────────────────────────────────────────────────

    private static async Task SeedSampleGrammarAsync(AppDbContext db)
    {
        if (await db.GrammarTopics.AnyAsync())
            return;

        var topics = new List<GrammarTopic>
        {
            new() { Id = Guid.NewGuid(), Title = "Simple Present Tense", Content = "The simple present tense is used to describe habits, unchanging situations, general truths, and fixed arrangements.\n\n**Structure:**\n- I/You/We/They + base verb\n- He/She/It + base verb + s/es\n\n**Examples:**\n- I eat breakfast every morning.\n- She works at a hospital.\n- The sun rises in the east.", Level = GrammarLevel.Beginner, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Title = "Present Continuous Tense", Content = "The present continuous tense is used to describe actions happening right now or around the time of speaking.\n\n**Structure:**\n- Subject + am/is/are + verb-ing\n\n**Examples:**\n- I am studying English now.\n- They are playing football.\n- She is working from home today.", Level = GrammarLevel.Beginner, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Title = "Conditional Sentences Type 1", Content = "Type 1 conditionals express real or possible situations in the present or future.\n\n**Structure:**\n- If + Simple Present, will + base verb\n\n**Examples:**\n- If it rains, I will stay home.\n- If you study hard, you will pass the exam.\n- If she calls, tell her I'm busy.", Level = GrammarLevel.Intermediate, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
        };

        db.GrammarTopics.AddRange(topics);
        await db.SaveChangesAsync();
    }
}
