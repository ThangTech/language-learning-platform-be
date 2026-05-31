using LanguagePlatform.Domain.Entities;
using LanguagePlatform.Domain.Enums;
using LanguagePlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace LanguagePlatform.Infrastructure.Persistence;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        await db.Database.MigrateAsync();

        await SeedAdminUserAsync(db);
        await CleanupAllListeningDataAsync(db);
        await SeedVstepExamListeningAsync(db);
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

    // ── Cleanup all old listening data ────────────────────────────────────────

    private static async Task CleanupAllListeningDataAsync(AppDbContext db)
    {
        var hasVstepMarker = await db.ListeningLessons
            .AnyAsync(l => l.Title == "VSTEP Listening Part 1 - Short Announcements");

        if (hasVstepMarker)
            return;

        var allLessons = await db.ListeningLessons.ToListAsync();
        if (allLessons.Count == 0)
            return;

        var lessonIds = allLessons.Select(l => l.Id).ToList();

        var lessonQuizzes = await db.Quizzes
            .Where(q => q.LessonId != null && lessonIds.Contains(q.LessonId.Value))
            .ToListAsync();
        var lessonDictationSets = await db.DictationSets
            .Where(d => d.LessonId != null && lessonIds.Contains(d.LessonId.Value))
            .ToListAsync();

        db.Quizzes.RemoveRange(lessonQuizzes);
        db.DictationSets.RemoveRange(lessonDictationSets);
        db.ListeningLessons.RemoveRange(allLessons);

        await db.SaveChangesAsync();
    }

    // ── VSTEP Exam Listening (3 Parts) ────────────────────────────────────────

    private static async Task SeedVstepExamListeningAsync(AppDbContext db)
    {
        const string markerTitle = "VSTEP Listening Part 1 - Short Announcements";

        if (await db.ListeningLessons.AnyAsync(lesson => lesson.Title == markerTitle))
            return;

        var now = DateTime.UtcNow;
        var groups = new List<VstepListeningGroup>
        {
            new(
                1,
                "VSTEP Listening Part 1 - Short Announcements",
                "Eight short announcements and instructions with one question for each audio.",
                "/audio/vstep/part1_q1.mp3",
                "B1",
                "VSTEP",
                102,
                new[]
                {
                    T("0:00", "Announcer", "Attention visitors. Starting this Friday, the City Museum will remain open until 9:00 PM every Friday evening. Regular closing times on other weekdays will stay the same."),
                    T("0:14", "Announcer", "Passengers traveling on flight SQ318 to Singapore, please note that your departure gate has changed. The flight will now depart from Gate 20, not Gate 15 as previously announced."),
                    T("0:27", "Announcer", "This is a reminder for all office staff. The IT department will upgrade the internal computer system tonight from 8:00 PM, so some online services may be unavailable for several hours."),
                    T("0:40", "Announcer", "Registration for this year's city marathon is now open. All participants must register online through the official marathon website before midnight on Sunday."),
                    T("0:52", "Announcer", "The guest lecture on modern history has been moved from Wednesday afternoon to Thursday morning. It will begin at 10:00 AM in Room 204 of the main building."),
                    T("1:04", "Announcer", "A new coffee shop has opened next to the library. Besides fresh coffee and homemade cakes, it will offer live acoustic music every Saturday evening."),
                    T("1:16", "Announcer", "Drivers are advised that Main Street will be closed for road repairs starting tonight. The road is expected to reopen on Friday morning, depending on weather conditions."),
                    T("1:28", "Announcer", "All new employees are reminded that they must complete the online training module before their first working day. Login details have been sent to your registered email address.")
                },
                new[]
                {
                    Q(1, "What is the new closing time for the City Museum on Fridays?", "9:00 PM", "The announcement says the museum will remain open until 9:00 PM every Friday evening.", "6:00 PM", "7:00 PM", "8:00 PM", "9:00 PM"),
                    Q(2, "Which gate does the flight to Singapore depart from?", "Gate 20", "The announcement states that the flight will now depart from Gate 20.", "Gate 10", "Gate 15", "Gate 20", "Gate 25"),
                    Q(3, "What is the main purpose of the announcement?", "To inform about an IT system upgrade", "The announcement tells staff that the IT department will upgrade the internal computer system.", "To announce a staff meeting", "To inform about an IT system upgrade", "To advertise a new product", "To report a security issue"),
                    Q(4, "Where should participants register for the marathon?", "Online", "Participants are told to register online through the official marathon website.", "At the main entrance", "Online", "At the City Hall", "At the sports center"),
                    Q(5, "When will the lecture on modern history be held?", "Thursday morning", "The announcement says the lecture has been moved to Thursday morning.", "Tuesday morning", "Wednesday afternoon", "Thursday morning", "Friday afternoon"),
                    Q(6, "What is a special feature of the new coffee shop?", "Live acoustic music", "The announcement says the shop will offer live acoustic music every Saturday evening.", "Free Wi-Fi access", "A selection of imported teas", "Live acoustic music", "Discounted pastries"),
                    Q(7, "How long will the road closure on Main Street last?", "Until Friday morning", "The announcement says the road is expected to reopen on Friday morning.", "Until the end of the day", "For 24 hours", "For the entire week", "Until Friday morning"),
                    Q(8, "What must new employees complete before starting work?", "An online training module", "New employees must complete the online training module before their first working day.", "A fitness test", "An online training module", "A background check", "A face-to-face interview")
                }),
            new(
                2,
                "VSTEP Listening Part 2 - Conversation 1",
                "A conversation between two friends choosing a birthday gift for Sarah.",
                "/audio/vstep/part2_conv1.mp3",
                "B1",
                "VSTEP",
                82,
                new[]
                {
                    T("0:00", "Man", "Sarah's birthday is this Saturday, and I still haven't bought her anything. I was thinking of getting her a book. She reads a lot, doesn't she?"),
                    T("0:09", "Woman", "She does, but that's exactly the problem. She already owns so many books that it would be hard to choose one she hasn't read."),
                    T("0:18", "Man", "That's true. I don't want to buy something she already has. What about a piece of jewelry?"),
                    T("0:25", "Woman", "It might be nice, but good jewelry can be expensive, and we agreed to keep the gift simple."),
                    T("0:33", "Man", "Right. Maybe a scarf, then? I saw some beautiful ones in the shop near campus."),
                    T("0:40", "Woman", "A scarf sounds perfect. Sarah loves fashion, and winter is coming soon, so it would be useful as well."),
                    T("0:49", "Man", "Great. Should we go tonight after class?"),
                    T("0:53", "Woman", "I can't tonight. I have to finish my presentation. How about tomorrow morning?"),
                    T("1:00", "Man", "Tomorrow morning works for me. Let's meet at ten outside the bookstore and then go to the shop together."),
                    T("1:09", "Woman", "Perfect. I'm sure Sarah will like it.")
                },
                new[]
                {
                    Q(9, "What was the man initially thinking of buying for Sarah?", "A book", "At the beginning, the man says he was thinking of getting Sarah a book.", "A watch", "A scarf", "A book", "A piece of jewelry"),
                    Q(10, "Why does the woman reject the man's first idea?", "Sarah already owns many of them.", "The woman says Sarah already owns so many books.", "It is too expensive.", "Sarah doesn't enjoy that item.", "Sarah already owns many of them.", "It is not suitable for the season."),
                    Q(11, "Why do the speakers decide that a scarf is a good choice?", "Sarah loves fashion, and winter is approaching.", "The woman says Sarah loves fashion and winter is coming soon.", "It fits the man's budget.", "Sarah needs one for her job.", "Sarah loves fashion, and winter is approaching.", "They can buy it tonight."),
                    Q(12, "When will the speakers go to buy the gift?", "Tomorrow morning", "They agree to meet tomorrow morning at ten.", "Tonight", "Next week", "Tomorrow morning", "Tomorrow afternoon")
                }),
            new(
                3,
                "VSTEP Listening Part 3 - Talk 1",
                "A short academic talk about digital citizenship.",
                "/audio/vstep/part3_talk1.mp3",
                "B2",
                "VSTEP",
                135,
                new[]
                {
                    T("0:00", "Speaker", "Good morning, everyone. Today I would like to talk about digital citizenship, a topic that has become increasingly important in our daily lives."),
                    T("0:10", "Speaker", "Most of us use the internet for studying, working, shopping, and communicating with others. However, being online is not just about using technology."),
                    T("0:22", "Speaker", "It is also about behaving responsibly, protecting ourselves, and respecting other people."),
                    T("0:30", "Speaker", "One important part of digital citizenship is online privacy. Some people believe that websites automatically protect all personal information, but this is not always true."),
                    T("0:43", "Speaker", "Users need to actively manage their privacy settings, choose carefully what they share, and understand who can see their posts, photos, or contact details."),
                    T("0:58", "Speaker", "Another issue is cyberbullying. If you see or experience harmful comments, threats, or repeated online harassment, the best response is not to start an argument in public."),
                    T("1:13", "Speaker", "Instead, you should report the content to the platform and block the user. It is also helpful to keep evidence and speak to a trusted person if the situation continues."),
                    T("1:28", "Speaker", "We should also remember that our online activities create a digital footprint. This means a permanent record of what we post, share, search for, and comment on."),
                    T("1:43", "Speaker", "Even when something is deleted, it may still be saved, copied, or seen by others. For this reason, digital citizens must think carefully about their online behavior."),
                    T("1:58", "Speaker", "To conclude, technology gives us many opportunities, but it also gives us responsibilities. My main advice is simple: think before posting."),
                    T("2:10", "Speaker", "A few seconds of careful thought can protect your privacy, your reputation, and the feelings of other people.")
                },
                new[]
                {
                    Q(21, "What is the talk mainly about?", "The importance of digital citizenship", "The speaker introduces digital citizenship as the main topic and discusses responsible online behavior.", "The history of the internet", "The dangers of social media", "The importance of digital citizenship", "New technology trends"),
                    Q(22, "What does the speaker emphasize about online privacy?", "Users need to actively manage their settings.", "The speaker says users need to actively manage their privacy settings.", "It is guaranteed by most websites.", "It is less important for younger users.", "Users need to actively manage their settings.", "Personal information is rarely shared."),
                    Q(23, "What is a suggested way to combat cyberbullying?", "Reporting the content and blocking the user", "The speaker advises reporting the content to the platform and blocking the user.", "Ignoring the bully", "Posting a public response", "Reporting the content and blocking the user", "Asking friends to intervene"),
                    Q(24, "According to the speaker, what is a 'digital footprint'?", "The permanent record of one's online activities.", "The talk defines a digital footprint as a permanent record of online activities.", "The total amount of time spent online.", "The unique search history of a user.", "The permanent record of one's online activities.", "The number of followers a person has."),
                    Q(25, "What is the speaker's concluding advice?", "To think before posting", "The speaker concludes with the advice to think before posting.", "To limit screen time", "To think before posting", "To use strong passwords only", "To avoid all social media")
                })
        };

        await InsertVstepGroupsAsync(db, groups, now);
    }

    // ── Insert VSTEP groups ───────────────────────────────────────────────────

    private static async Task InsertVstepGroupsAsync(
        AppDbContext db,
        List<VstepListeningGroup> groups,
        DateTime now)
    {
        var lessons = new List<ListeningLesson>();
        var quizzes = new List<Quiz>();
        var dictationSets = new List<DictationSet>();

        int index = 0;
        foreach (var group in groups)
        {
            var createdAt = now.AddMinutes(-group.Part).AddSeconds(-index);
            var lesson = new ListeningLesson
            {
                Id = Guid.NewGuid(),
                Title = group.Title,
                Description = group.Description,
                AudioUrl = group.AudioUrl,
                Level = group.Level,
                Topic = group.Topic,
                Duration = group.Duration,
                TranscriptJson = ToTranscriptJson(group.Transcript),
                CreatedAt = createdAt,
                UpdatedAt = createdAt
            };

            lessons.Add(lesson);

            quizzes.Add(new Quiz
            {
                Id = Guid.NewGuid(),
                Title = $"Quiz: {group.Title}",
                LessonId = lesson.Id,
                Difficulty = group.Part == 3 ? QuizDifficulty.Hard : QuizDifficulty.Medium,
                Type = QuizType.MultipleChoice,
                DurationMinutes = group.Part == 1 ? 15 : group.Part == 2 ? 10 : 15,
                Questions = group.Questions.Select(question => CreateListeningQuestion(question, group.AudioUrl)).ToList()
            });

            dictationSets.Add(new DictationSet
            {
                Id = Guid.NewGuid(),
                Title = $"Chép chính tả: {group.Title}",
                Description = $"Nghe audio {group.Title} và gõ lại nội dung transcript.",
                Level = group.Level,
                Topic = group.Topic,
                LessonId = lesson.Id,
                Sentences = group.Transcript.Select((line, lineIdx) => new DictationSentence
                {
                    Id = Guid.NewGuid(),
                    Sentence = line.Text,
                    AudioTitle = $"{group.Title} - Segment {lineIdx + 1}",
                    AudioUrl = group.AudioUrl,
                    Hint = BuildProgressiveHint(line.Text, 4),
                    Duration = EstimateSentenceDuration(line.Text),
                    OrderIndex = lineIdx + 1
                }).ToList()
            });
            index++;
        }

        db.ListeningLessons.AddRange(lessons);
        db.Quizzes.AddRange(quizzes);
        db.DictationSets.AddRange(dictationSets);
        await db.SaveChangesAsync();
    }

    // ── Helper methods ────────────────────────────────────────────────────────

    private static QuizQuestion CreateListeningQuestion(
        VstepQuestion question,
        string audioUrl)
    {
        return new QuizQuestion
        {
            Id = Guid.NewGuid(),
            QuestionText = question.Text,
            Type = QuestionType.MultipleChoice,
            Options = question.Options.ToList(),
            CorrectAnswer = question.CorrectAnswer,
            Explanation = question.Explanation,
            AudioUrl = audioUrl
        };
    }

    private static string BuildProgressiveHint(string text, int visibleWords)
    {
        var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (words.Length <= visibleWords)
            return text;

        return string.Join(' ', words.Take(visibleWords)) + " ...";
    }

    private static VstepQuestion Q(
        int number,
        string text,
        string correctAnswer,
        string explanation,
        params string[] options)
        => new(number, text, correctAnswer, explanation, options);

    private static TranscriptLine T(
        string time,
        string speaker,
        string text)
        => new(time, speaker, text);

    private static string ToTranscriptJson(IEnumerable<TranscriptLine> lines)
        => JsonSerializer.Serialize(lines.Select(line => new
        {
            time = line.Time,
            speaker = line.Speaker,
            text = line.Text
        }));

    private static int EstimateSentenceDuration(string text)
    {
        var wordCount = text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
        return Math.Max(4, (int)Math.Ceiling(wordCount / 2.5));
    }

    private sealed record TranscriptLine(
        string Time,
        string Speaker,
        string Text);

    private sealed record VstepQuestion(
        int Number,
        string Text,
        string CorrectAnswer,
        string Explanation,
        string[] Options);

    private sealed record VstepListeningGroup(
        int Part,
        string Title,
        string Description,
        string AudioUrl,
        string Level,
        string Topic,
        int Duration,
        TranscriptLine[] Transcript,
        VstepQuestion[] Questions);
}
