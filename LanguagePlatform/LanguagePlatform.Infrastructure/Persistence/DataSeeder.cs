using LanguagePlatform.Domain.Entities;
using LanguagePlatform.Domain.Enums;
using LanguagePlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace LanguagePlatform.Infrastructure.Persistence;

public static class DataSeeder
{
    private static readonly Guid SimplePresentId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private static readonly Guid PresentContinuousId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    private static readonly Guid ConditionalType1Id = Guid.Parse("33333333-3333-3333-3333-333333333333");

    public static async Task SeedAsync(AppDbContext db)
    {
        // Chạy pending migrations trước
        await db.Database.MigrateAsync();

        await SeedAdminUserAsync(db);
        await SeedSampleWordsAsync(db);
        await SeedSampleGrammarAsync(db);
        await SeedSampleQuizzesAsync(db);
        await CleanupLegacyListeningSeedAsync(db);
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

    // ── Sample Words ──────────────────────────────────────────────────────────

    private static async Task SeedSampleWordsAsync(AppDbContext db)
    {
        var hasEnglishWord = await db.Words.AnyAsync(w => w.Term == "ability" && w.Definition.Contains("physical"));
        if (hasEnglishWord)
        {
            db.Words.RemoveRange(db.Words);
            await db.SaveChangesAsync();
        }

        if (await db.Words.AnyAsync())
            return;

        var words = new List<Word>
        {
            new() { Id = Guid.NewGuid(), Term = "abandon", Definition = "từ bỏ, ruồng bỏ, bỏ rơi", ExampleSentence = "She had to abandon her car in the snow.", Level = WordLevel.Intermediate, Topic = "General", ImageUrl = null, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Term = "ability", Definition = "khả năng, năng lực", ExampleSentence = "She has the ability to solve complex problems.", Level = WordLevel.Beginner, Topic = "General", ImageUrl = null, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Term = "abolish", Definition = "bãi bỏ, thủ tiêu, hủy bỏ", ExampleSentence = "Slavery was abolished in the US in 1865.", Level = WordLevel.Advanced, Topic = "History", ImageUrl = null, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Term = "abstract", Definition = "trừu tượng", ExampleSentence = "Love is an abstract concept.", Level = WordLevel.Intermediate, Topic = "Academic", ImageUrl = null, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Term = "accelerate", Definition = "tăng tốc, làm nhanh hơn", ExampleSentence = "The car accelerated down the highway.", Level = WordLevel.Intermediate, Topic = "Science", ImageUrl = null, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Term = "accuse", Definition = "buộc tội, kết tội", ExampleSentence = "He was accused of stealing the money.", Level = WordLevel.Beginner, Topic = "General", ImageUrl = null, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Term = "achieve", Definition = "đạt được, giành được", ExampleSentence = "You can achieve anything with hard work.", Level = WordLevel.Beginner, Topic = "General", ImageUrl = null, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Term = "adjust", Definition = "điều chỉnh, dàn xếp", ExampleSentence = "It took her a while to adjust to the new environment.", Level = WordLevel.Beginner, Topic = "General", ImageUrl = null, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new() { Id = Guid.NewGuid(), Term = "affect", Definition = "ảnh hưởng, tác động đến", ExampleSentence = "The weather affects our mood.", Level = WordLevel.Beginner, Topic = "General", ImageUrl = null, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
        };

        db.Words.AddRange(words);
        await db.SaveChangesAsync();
    }

    // ── Sample Grammar Topics ─────────────────────────────────────────────────

    private static async Task SeedSampleGrammarAsync(AppDbContext db)
    {
        var hasYouTube = await db.GrammarTopics.AnyAsync(t => t.YouTubeUrl != null);
        if (!hasYouTube && await db.GrammarTopics.AnyAsync())
        {
            db.GrammarTopics.RemoveRange(db.GrammarTopics);
            await db.SaveChangesAsync();
        }

        if (await db.GrammarTopics.AnyAsync())
            return;

        var topics = new List<GrammarTopic>
        {
            new() {
                Id = SimplePresentId,
                Title = "Simple Present Tense",
                Content = "The simple present tense is used to describe habits, unchanging situations, general truths, and fixed arrangements.\n\n**Structure:**\n- I/You/We/They + base verb\n- He/She/It + base verb + s/es\n\n**Examples:**\n- I eat breakfast every morning.\n- She works at a hospital.\n- The sun rises in the east.",
                Explanation = "Thì hiện tại đơn diễn tả một hành động lặp đi lặp lại theo thói quen, một sự thật hiển nhiên hoặc một lịch trình cố định.",
                Examples = "- My father goes to work at 7 AM daily.\n- Water boils at 100 degrees Celsius.\n- The train leaves at 8:00 PM.",
                Level = GrammarLevel.Beginner,
                YouTubeUrl = "https://www.youtube.com/watch?v=L9AJuOC_19Y",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new() {
                Id = PresentContinuousId,
                Title = "Present Continuous Tense",
                Content = "The present continuous tense is used to describe actions happening right now or around the time of speaking.\n\n**Structure:**\n- Subject + am/is/are + verb-ing\n\n**Examples:**\n- I am studying English now.\n- They are playing football.\n- She is working from home today.",
                Explanation = "Thì hiện tại tiếp diễn được sử dụng để diễn tả một hành động đang xảy ra ngay tại thời điểm nói hoặc xung quanh thời điểm nói.",
                Examples = "- Look! It is snowing outside.\n- The children are making a snowman.\n- What are you doing at the moment?",
                Level = GrammarLevel.Beginner,
                YouTubeUrl = "https://www.youtube.com/watch?v=0k5Gki6pZ3Q",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new() {
                Id = ConditionalType1Id,
                Title = "Conditional Sentences Type 1",
                Content = "Type 1 conditionals express real or possible situations in the present or future.\n\n**Structure:**\n- If + Simple Present, will + base verb\n\n**Examples:**\n- If it rains, I will stay home.\n- If you study hard, you will pass the exam.\n- If she calls, tell her I'm busy.",
                Explanation = "Câu điều kiện loại 1 dùng để diễn tả một sự việc có thể xảy ra ở hiện tại hoặc tương lai nếu có một điều kiện nhất định xảy ra trước.",
                Examples = "- If you don't hurry, you will miss the train.\n- If she gets the job, she will move to Hanoi.\n- If they invite us, we will come.",
                Level = GrammarLevel.Intermediate,
                YouTubeUrl = "https://www.youtube.com/watch?v=rUj7K9lAEvc",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
        };

        db.GrammarTopics.AddRange(topics);
        await db.SaveChangesAsync();
    }

    // ── Sample Listening Lessons ────────────────────────────────────────────────

    private static async Task SeedSampleListeningAsync(AppDbContext db)
    {
        if (await db.ListeningLessons.AnyAsync())
        {
            return;
        }

        var now = DateTime.UtcNow;

        var airportLesson = new ListeningLesson
        {
            Id = Guid.NewGuid(),
            Title = "Thông báo tại sân bay",
            Description = "Bài nghe ngắn mẫu VSTEP về thông báo thay đổi cổng bay.",
            AudioUrl = "",
            Level = "A1",
            Topic = "Travel",
            Duration = 75,
            TranscriptJson = "[{\"time\":\"0:00\",\"speaker\":\"Announcer\",\"text\":\"Attention please. Flight BA 245 to London will now leave from Gate 12.\"},{\"time\":\"0:12\",\"speaker\":\"Announcer\",\"text\":\"Passengers should go to the gate now. Boarding starts in ten minutes.\"}]",
            CreatedAt = now,
            UpdatedAt = now
        };

        var conversationLesson = new ListeningLesson
        {
            Id = Guid.NewGuid(),
            Title = "Hội thoại tại nhà sách",
            Description = "Bài nghe hội thoại ngắn giữa học sinh và nhân viên bán hàng.",
            AudioUrl = "",
            Level = "A2",
            Topic = "Shopping",
            Duration = 130,
            TranscriptJson = "[{\"time\":\"0:00\",\"speaker\":\"Student\",\"text\":\"Excuse me, do you have English grammar books for beginners?\"},{\"time\":\"0:08\",\"speaker\":\"Assistant\",\"text\":\"Yes. They are on the second shelf near the window.\"}]",
            CreatedAt = now,
            UpdatedAt = now
        };

        var talkLesson = new ListeningLesson
        {
            Id = Guid.NewGuid(),
            Title = "Bài nói về thói quen học hàng ngày",
            Description = "Bài nói ngắn về cách xây dựng thói quen học tiếng Anh hàng ngày.",
            AudioUrl = "",
            Level = "B1",
            Topic = "Study",
            Duration = 180,
            TranscriptJson = "[{\"time\":\"0:00\",\"speaker\":\"Speaker\",\"text\":\"Learning English every day does not need to take many hours.\"},{\"time\":\"0:10\",\"speaker\":\"Speaker\",\"text\":\"A good plan is to study vocabulary, listen to short talks, and review mistakes.\"}]",
            CreatedAt = now,
            UpdatedAt = now
        };

        var lectureLesson = new ListeningLesson
        {
            Id = Guid.NewGuid(),
            Title = "Bài giảng nhỏ về học trực tuyến",
            Description = "Bài nghe dạng bài giảng B2 về lợi ích của học trực tuyến.",
            AudioUrl = "",
            Level = "B2",
            Topic = "Education",
            Duration = 240,
            TranscriptJson = "[{\"time\":\"0:00\",\"speaker\":\"Lecturer\",\"text\":\"Online learning has changed the way students access education.\"},{\"time\":\"0:14\",\"speaker\":\"Lecturer\",\"text\":\"It gives learners more flexibility, but it also requires self-discipline and clear goals.\"}]",
            CreatedAt = now,
            UpdatedAt = now
        };

        db.ListeningLessons.AddRange(
            airportLesson,
            conversationLesson,
            talkLesson,
            lectureLesson);

        var dictationSet = new DictationSet
        {
            Id = Guid.NewGuid(),
            Title = "Chép chính tả: chỉ dẫn tại sân bay",
            Description = "Nghe các câu ngắn tại sân bay và gõ lại nội dung.",
            Level = "A2",
            Topic = "Travel",
            LessonId = airportLesson.Id,
            Sentences = new List<DictationSentence>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Sentence = "The flight to London leaves from gate twelve.",
                    AudioTitle = "Câu 1 - Thay đổi cổng",
                    Hint = "Nghe để bắt số cổng và thành phố.",
                    Duration = 8,
                    OrderIndex = 1
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Sentence = "Please keep your passport ready for boarding.",
                    AudioTitle = "Câu 2 - Hướng dẫn lên máy bay",
                    Hint = "Nghe để biết loại giấy tờ cần chuẩn bị.",
                    Duration = 7,
                    OrderIndex = 2
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Sentence = "Boarding starts in ten minutes.",
                    AudioTitle = "Câu 3 - Thông báo thời gian",
                    Hint = "Nghe để bắt biểu thức thời gian.",
                    Duration = 6,
                    OrderIndex = 3
                }
            }
        };

        db.DictationSets.Add(dictationSet);
        await db.SaveChangesAsync();
    }

    // ── Sample Listening & Grammar Quizzes ───────────────────────────────────────

    private static async Task SeedSampleQuizzesAsync(AppDbContext db)
    {
        var lessons = await db.ListeningLessons.ToListAsync();
        var quizzes = new List<Quiz>();

        AddQuizIfMissing(
            db,
            quizzes,
            lessons,
            "Thông báo tại sân bay",
            "Bài quiz: thông báo tại sân bay",
            QuizDifficulty.Easy,
            QuizType.MultipleChoice,
            8,
            new List<QuizQuestion>
            {
                CreateMultipleChoiceQuestion(
                    "Where will the flight to London leave from?",
                    "Gate 12",
                    "The announcement says the flight leaves from Gate 12.",
                    "Gate 10",
                    "Gate 12",
                    "Gate 20"),
                CreateMultipleChoiceQuestion(
                    "When does boarding start?",
                    "In ten minutes",
                    "The speaker says boarding starts in ten minutes.",
                    "In five minutes",
                    "In ten minutes",
                    "In twenty minutes")
            });

        AddQuizIfMissing(
            db,
            quizzes,
            lessons,
            "Hội thoại tại nhà sách",
            "Điền từ: hội thoại nhà sách",
            QuizDifficulty.Medium,
            QuizType.FillInBlank,
            10,
            new List<QuizQuestion>
            {
                CreateFillInBlankQuestion(
                    "The student asks for English grammar books for ______.",
                    "beginners",
                    "The student says: grammar books for beginners."),
                CreateFillInBlankQuestion(
                    "The books are on the second shelf near the ______.",
                    "window",
                    "The assistant says the books are near the window.")
            });

        AddQuizIfMissing(
            db,
            quizzes,
            lessons,
            "Bài nói về thói quen học hàng ngày",
            "Bài quiz: thói quen học hàng ngày",
            QuizDifficulty.Medium,
            QuizType.MultipleChoice,
            12,
            new List<QuizQuestion>
            {
                CreateMultipleChoiceQuestion(
                    "What is the main idea of the talk?",
                    "Study a little every day",
                    "The talk explains how daily study builds a better habit.",
                    "Study only on weekends",
                    "Study a little every day",
                    "Stop reviewing mistakes"),
                CreateMultipleChoiceQuestion(
                    "What should learners review?",
                    "Mistakes",
                    "The speaker says learners should review mistakes.",
                    "Only new words",
                    "Mistakes",
                    "Movies")
            });

        AddQuizIfMissing(
            db,
            quizzes,
            lessons,
            "Bài giảng nhỏ về học trực tuyến",
            "Bài quiz: bài giảng học trực tuyến",
            QuizDifficulty.Hard,
            QuizType.MultipleChoice,
            15,
            new List<QuizQuestion>
            {
                CreateMultipleChoiceQuestion(
                    "What has online learning changed?",
                    "How students access education",
                    "The lecturer says online learning changed access to education.",
                    "How students buy food",
                    "How students access education",
                    "How students travel"),
                CreateMultipleChoiceQuestion(
                    "What does online learning require?",
                    "Self-discipline and clear goals",
                    "The lecturer says it requires self-discipline and clear goals.",
                    "More classrooms",
                    "Self-discipline and clear goals",
                    "Longer holidays")
            });

        if (quizzes.Count > 0)
        {
            db.Quizzes.AddRange(quizzes);
            await db.SaveChangesAsync();
        }

        // Grammar Quizzes
        var hasGrammarQuiz = await db.Quizzes.AnyAsync(q => q.GrammarTopicId != null);
        if (!hasGrammarQuiz)
        {
            var grammarQuizzes = new List<Quiz>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Trắc nghiệm Simple Present Tense",
                    GrammarTopicId = SimplePresentId,
                    Difficulty = QuizDifficulty.Easy,
                    Type = QuizType.MultipleChoice,
                    DurationMinutes = 10,
                    Questions = new List<QuizQuestion>
                    {
                        CreateMultipleChoiceQuestion(
                            "Choose the correct option: She ______ (go) to school everyday.",
                            "goes",
                            "With singular third person pronoun (She/He/It), simple present verbs add 's/es'.",
                            "go", "goes", "going", "went"),
                        CreateMultipleChoiceQuestion(
                            "Choose the correct option: They ______ (play) football on Sundays.",
                            "play",
                            "With plural pronouns (They/We/You/I), the simple present uses the base form of the verb.",
                            "play", "plays", "playing", "played")
                    }
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Trắc nghiệm Present Continuous Tense",
                    GrammarTopicId = PresentContinuousId,
                    Difficulty = QuizDifficulty.Easy,
                    Type = QuizType.MultipleChoice,
                    DurationMinutes = 10,
                    Questions = new List<QuizQuestion>
                    {
                        CreateMultipleChoiceQuestion(
                            "Choose the correct option: I ______ (study) English right now.",
                            "am studying",
                            "Present continuous structure: subject + am/is/are + verb-ing. 'I' takes 'am studying'.",
                            "am studying", "is studying", "are studying", "study"),
                        CreateMultipleChoiceQuestion(
                            "Choose the correct option: Listen! The birds ______ (sing) in the tree.",
                            "are singing",
                            "The action is happening right now, and 'birds' is plural, so we use 'are singing'.",
                            "sings", "is singing", "are singing", "sang")
                    }
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "Trắc nghiệm Conditional Sentences Type 1",
                    GrammarTopicId = ConditionalType1Id,
                    Difficulty = QuizDifficulty.Medium,
                    Type = QuizType.MultipleChoice,
                    DurationMinutes = 15,
                    Questions = new List<QuizQuestion>
                    {
                        CreateMultipleChoiceQuestion(
                            "Complete the sentence: If it ______ (rain) tomorrow, we will stay at home.",
                            "rains",
                            "Conditional Type 1 uses Simple Present in the 'if' clause: 'If it rains...'.",
                            "rain", "rains", "will rain", "rained"),
                        CreateMultipleChoiceQuestion(
                            "Complete the sentence: If you ______ (not study) hard, you won't pass the exam.",
                            "don't study",
                            "We use simple present negation for 'you' in the 'if' clause: 'If you don't study...'.",
                            "don't study", "doesn't study", "won't study", "didn't study")
                    }
                }
            };
            db.Quizzes.AddRange(grammarQuizzes);
            await db.SaveChangesAsync();
        }
    }

    private static async Task CleanupLegacyListeningSeedAsync(AppDbContext db)
    {
        var legacyLessons = await db.ListeningLessons
            .Where(lesson =>
                (
                    lesson.AudioUrl == "" &&
                    lesson.TranscriptJson != null &&
                    (
                        lesson.TranscriptJson.Contains("Flight BA 245") ||
                        lesson.TranscriptJson.Contains("English grammar books") ||
                        lesson.TranscriptJson.Contains("Learning English every day") ||
                        lesson.TranscriptJson.Contains("Online learning has changed")
                    )
                ) ||
                lesson.Title.StartsWith("VSTEP Part 1 - Question") ||
                lesson.Title.StartsWith("VSTEP Part 2 - Conversation 1") ||
                lesson.Title.StartsWith("VSTEP Part 3 - Talk 1"))
            .ToListAsync();

        if (legacyLessons.Count == 0)
        {
            return;
        }

        var legacyLessonIds = legacyLessons.Select(lesson => lesson.Id).ToList();
        var legacyQuizzes = await db.Quizzes
            .Where(quiz => quiz.LessonId != null && legacyLessonIds.Contains(quiz.LessonId.Value))
            .ToListAsync();
        var legacyDictationSets = await db.DictationSets
            .Where(set => set.LessonId != null && legacyLessonIds.Contains(set.LessonId.Value))
            .ToListAsync();

        db.Quizzes.RemoveRange(legacyQuizzes);
        db.DictationSets.RemoveRange(legacyDictationSets);
        db.ListeningLessons.RemoveRange(legacyLessons);

        await db.SaveChangesAsync();
    }

    private static async Task SeedVstepListeningAsync(AppDbContext db)
    {
        const string markerTitle = "VSTEP Part 1 - Question 1: City Museum Friday Hours";

        if (await db.ListeningLessons.AnyAsync(lesson => lesson.Title == markerTitle))
        {
            return;
        }

        var now = DateTime.UtcNow;
        var groups = new List<VstepListeningGroup>
        {
            new(
                1,
                "VSTEP Part 1 - Question 1: City Museum Friday Hours",
                "Short announcement about the City Museum's updated Friday opening hours.",
                "/audio/vstep/part1_q1.mp3",
                "B1",
                "Announcements",
                14,
                new[] { T("0:00", "Announcer", "Attention visitors. Starting this Friday, the City Museum will remain open until 9:00 PM every Friday evening. Regular closing times on other weekdays will stay the same.") },
                new[]
                {
                    Q(1, "What is the new closing time for the City Museum on Fridays?", "9:00 PM", "The announcement says the museum will remain open until 9:00 PM every Friday evening.", "6:00 PM", "7:00 PM", "8:00 PM", "9:00 PM")
                }),
            new(
                1,
                "VSTEP Part 1 - Question 2: Singapore Flight Gate Change",
                "Airport announcement about a gate change for a flight to Singapore.",
                "/audio/vstep/part1_q2.mp3",
                "B1",
                "Travel",
                13,
                new[] { T("0:00", "Announcer", "Passengers traveling on flight SQ318 to Singapore, please note that your departure gate has changed. The flight will now depart from Gate 20, not Gate 15 as previously announced.") },
                new[]
                {
                    Q(2, "Which gate does the flight to Singapore depart from?", "Gate 20", "The announcement states that the flight will now depart from Gate 20.", "Gate 10", "Gate 15", "Gate 20", "Gate 25")
                }),
            new(
                1,
                "VSTEP Part 1 - Question 3: Office IT Upgrade",
                "Workplace announcement about an internal computer system upgrade.",
                "/audio/vstep/part1_q3.mp3",
                "B1",
                "Work",
                13,
                new[] { T("0:00", "Announcer", "This is a reminder for all office staff. The IT department will upgrade the internal computer system tonight from 8:00 PM, so some online services may be unavailable for several hours.") },
                new[]
                {
                    Q(3, "What is the main purpose of the announcement?", "To inform about an IT system upgrade", "The announcement tells staff that the IT department will upgrade the internal computer system.", "To announce a staff meeting", "To inform about an IT system upgrade", "To advertise a new product", "To report a security issue")
                }),
            new(
                1,
                "VSTEP Part 1 - Question 4: City Marathon Registration",
                "Public announcement about how to register for a city marathon.",
                "/audio/vstep/part1_q4.mp3",
                "B1",
                "Sports",
                12,
                new[] { T("0:00", "Announcer", "Registration for this year's city marathon is now open. All participants must register online through the official marathon website before midnight on Sunday.") },
                new[]
                {
                    Q(4, "Where should participants register for the marathon?", "Online", "Participants are told to register online through the official marathon website.", "At the main entrance", "Online", "At the City Hall", "At the sports center")
                }),
            new(
                1,
                "VSTEP Part 1 - Question 5: Modern History Lecture",
                "Campus announcement about a schedule change for a modern history lecture.",
                "/audio/vstep/part1_q5.mp3",
                "B1",
                "Education",
                12,
                new[] { T("0:00", "Announcer", "The guest lecture on modern history has been moved from Wednesday afternoon to Thursday morning. It will begin at 10:00 AM in Room 204 of the main building.") },
                new[]
                {
                    Q(5, "When will the lecture on modern history be held?", "Thursday morning", "The announcement says the lecture has been moved to Thursday morning.", "Tuesday morning", "Wednesday afternoon", "Thursday morning", "Friday afternoon")
                }),
            new(
                1,
                "VSTEP Part 1 - Question 6: New Coffee Shop Opening",
                "Local announcement about a new coffee shop near the library.",
                "/audio/vstep/part1_q6.mp3",
                "B1",
                "Lifestyle",
                12,
                new[] { T("0:00", "Announcer", "A new coffee shop has opened next to the library. Besides fresh coffee and homemade cakes, it will offer live acoustic music every Saturday evening.") },
                new[]
                {
                    Q(6, "What is a special feature of the new coffee shop?", "Live acoustic music", "The announcement says the shop will offer live acoustic music every Saturday evening.", "Free Wi-Fi access", "A selection of imported teas", "Live acoustic music", "Discounted pastries")
                }),
            new(
                1,
                "VSTEP Part 1 - Question 7: Main Street Road Closure",
                "Traffic announcement about repair work on Main Street.",
                "/audio/vstep/part1_q7.mp3",
                "B1",
                "Transport",
                12,
                new[] { T("0:00", "Announcer", "Drivers are advised that Main Street will be closed for road repairs starting tonight. The road is expected to reopen on Friday morning, depending on weather conditions.") },
                new[]
                {
                    Q(7, "How long will the road closure on Main Street last?", "Until Friday morning", "The announcement says the road is expected to reopen on Friday morning.", "Until the end of the day", "For 24 hours", "For the entire week", "Until Friday morning")
                }),
            new(
                1,
                "VSTEP Part 1 - Question 8: New Employee Requirement",
                "Human resources announcement for new employees.",
                "/audio/vstep/part1_q8.mp3",
                "B1",
                "Work",
                11,
                new[] { T("0:00", "Announcer", "All new employees are reminded that they must complete the online training module before their first working day. Login details have been sent to your registered email address.") },
                new[]
                {
                    Q(8, "What must new employees complete before starting work?", "An online training module", "New employees must complete the online training module before their first working day.", "A fitness test", "An online training module", "A background check", "A face-to-face interview")
                }),
            new(
                2,
                "VSTEP Part 2 - Conversation 1: Choosing a Birthday Gift for Sarah",
                "Two friends discuss what to buy for Sarah's birthday.",
                "/audio/vstep/part2_conv1.mp3",
                "B1",
                "Daily Life",
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
                "VSTEP Part 3 - Talk 1: Digital Citizenship",
                "A short academic talk about responsible behavior online.",
                "/audio/vstep/part3_talk1.mp3",
                "B2",
                "Technology",
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
                DurationMinutes = group.Part == 1 ? 5 : group.Part == 2 ? 10 : 15,
                Questions = group.Questions.Select(question => CreateListeningQuestion(question, group.AudioUrl)).ToList()
            });

            dictationSets.Add(new DictationSet
            {
                Id = Guid.NewGuid(),
                Title = $"Chép chính tả: {group.Title}",
                Description = $"Nghe audio {group.Title} và gõ lại các câu chính trong transcript.",
                Level = group.Level,
                Topic = group.Topic,
                LessonId = lesson.Id,
                Sentences = group.Transcript.Select((line, lineIndex) => new DictationSentence
                {
                    Id = Guid.NewGuid(),
                    Sentence = line.Text,
                    AudioTitle = $"{group.Title} - Sentence {lineIndex + 1}",
                    AudioUrl = group.AudioUrl,
                    Hint = $"Speaker: {line.Speaker}. Listen for the main sentence in this segment.",
                    Duration = EstimateSentenceDuration(line.Text),
                    OrderIndex = lineIndex + 1
                }).ToList()
            });
            index++;
        }

        db.ListeningLessons.AddRange(lessons);
        db.Quizzes.AddRange(quizzes);
        db.DictationSets.AddRange(dictationSets);
        await db.SaveChangesAsync();
    }

    private static async Task SeedVstepExamListeningAsync(AppDbContext db)
    {
        const string markerTitle = "VSTEP Listening Part 1 - Short Announcements";

        if (await db.ListeningLessons.AnyAsync(lesson => lesson.Title == markerTitle))
        {
            return;
        }

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

    private static void AddQuizIfMissing(
        AppDbContext db,
        List<Quiz> quizzes,
        List<ListeningLesson> lessons,
        string lessonTitle,
        string quizTitle,
        QuizDifficulty difficulty,
        QuizType type,
        int durationMinutes,
        List<QuizQuestion> questions)
    {
        var alreadyExists = db.Quizzes.Any(quiz => quiz.Title == quizTitle);

        if (alreadyExists)
        {
            return;
        }

        var lesson = lessons.FirstOrDefault(item => item.Title == lessonTitle);

        if (lesson == null)
        {
            return;
        }

        quizzes.Add(new Quiz
        {
            Id = Guid.NewGuid(),
            Title = quizTitle,
            LessonId = lesson.Id,
            Difficulty = difficulty,
            Type = type,
            DurationMinutes = durationMinutes,
            Questions = questions
        });
    }

    private static QuizQuestion CreateMultipleChoiceQuestion(
        string text,
        string answer,
        string explanation,
        params string[] options)
    {
        return new QuizQuestion
        {
            Id = Guid.NewGuid(),
            QuestionText = text,
            Type = QuestionType.MultipleChoice,
            Options = options.ToList(),
            CorrectAnswer = answer,
            Explanation = explanation
        };
    }

    private static QuizQuestion CreateFillInBlankQuestion(
        string text,
        string answer,
        string explanation)
    {
        return new QuizQuestion
        {
            Id = Guid.NewGuid(),
            QuestionText = text,
            Type = QuestionType.FillInBlank,
            Options = new List<string>(),
            CorrectAnswer = answer,
            Explanation = explanation
        };
    }

    private static QuizQuestion CreateListeningQuestion(
        VstepQuestion question,
        string audioUrl)
    {
        var questionAudioUrl = question.Number is >= 1 and <= 8
            ? $"/audio/vstep/part1_q{question.Number}.mp3"
            : audioUrl;

        return new QuizQuestion
        {
            Id = Guid.NewGuid(),
            QuestionText = question.Text,
            Type = QuestionType.MultipleChoice,
            Options = question.Options.ToList(),
            CorrectAnswer = question.CorrectAnswer,
            Explanation = question.Explanation,
            AudioUrl = questionAudioUrl
        };
    }

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
                    AudioUrl = group.Part == 1 ? $"/audio/vstep/part1_q{lineIdx + 1}.mp3" : group.AudioUrl,
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

    private static string BuildProgressiveHint(string text, int visibleWords)
    {
        var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (words.Length <= visibleWords)
        {
            return text;
        }

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
