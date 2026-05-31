using LanguagePlatform.Domain.Entities;
using LanguagePlatform.Domain.Enums;
using LanguagePlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

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
        await SeedSampleListeningAsync(db);
        await SeedSampleQuizzesAsync(db);
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
}
