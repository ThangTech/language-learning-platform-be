using AutoMapper;
using LanguagePlatform.Application.DTOs.Auth;
using LanguagePlatform.Application.DTOs.Grammar;
using LanguagePlatform.Application.DTOs.Listening;
using LanguagePlatform.Application.DTOs.Notification;
using LanguagePlatform.Application.DTOs.Progress;
using LanguagePlatform.Application.DTOs.Quiz;
using LanguagePlatform.Application.DTOs.Vocabulary;
using LanguagePlatform.Domain.Entities;

namespace LanguagePlatform.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Auth / User
        CreateMap<User, UserDto>()
            .ForMember(d => d.Role, o => o.MapFrom(s => s.Role.ToString()))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));
        CreateMap<RegisterRequest, User>()
            .ForMember(d => d.PasswordHash, o => o.Ignore())
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.UpdatedAt, o => o.Ignore());

        // Vocabulary
        CreateMap<Word, WordDto>()
            .ForMember(d => d.Level, o => o.MapFrom(s => s.Level.ToString()));
        CreateMap<CreateWordRequest, Word>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.UpdatedAt, o => o.Ignore());
        CreateMap<UpdateWordRequest, Word>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.UpdatedAt, o => o.Ignore());
        CreateMap<Flashcard, FlashcardDto>();

        // Grammar
        CreateMap<GrammarTopic, GrammarTopicDto>()
            .ForMember(d => d.Level, o => o.MapFrom(s => s.Level.ToString()));
        CreateMap<CreateGrammarTopicRequest, GrammarTopic>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.UpdatedAt, o => o.Ignore());
        CreateMap<UpdateGrammarTopicRequest, GrammarTopic>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.UpdatedAt, o => o.Ignore());
        CreateMap<UserGrammar, UserGrammarDto>();

        // Listening
        CreateMap<ListeningLesson, ListeningLessonDto>();
        CreateMap<CreateListeningLessonRequest, ListeningLesson>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.UpdatedAt, o => o.Ignore());
        CreateMap<UpdateListeningLessonRequest, ListeningLesson>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.UpdatedAt, o => o.Ignore());
        CreateMap<ListeningResult, ListeningResultDto>();
        CreateMap<DictationSet, DictationSetDto>();
        CreateMap<DictationSentence, DictationSentenceDto>();

        // Quiz
        CreateMap<Quiz, QuizDto>();
        CreateMap<QuizQuestion, QuizQuestionDto>()
            .ForMember(d => d.Type, o => o.MapFrom(s => s.Type.ToString()));
        CreateMap<CreateQuizRequest, Quiz>()
            .ForMember(d => d.Id, o => o.Ignore());
        CreateMap<CreateQuizQuestionRequest, QuizQuestion>()
            .ForMember(d => d.Id, o => o.Ignore());

        // Progress
        CreateMap<UserProgress, UserProgressDto>();
        CreateMap<UserProgress, StreakDto>()
            .ForMember(d => d.CurrentStreak, o => o.MapFrom(s => s.CurrentStreak))
            .ForMember(d => d.LongestStreak, o => o.MapFrom(s => s.LongestStreak))
            .ForMember(d => d.LastActivityDate, o => o.MapFrom(s => s.LastActivityDate));
        CreateMap<UserProgress, LeaderboardEntryDto>()
            .ForMember(d => d.FullName, o => o.MapFrom(s => s.User != null ? s.User.FullName : string.Empty))
            .ForMember(d => d.AvatarUrl, o => o.MapFrom(s => s.User != null ? s.User.AvatarUrl : null));

        // Notification
        CreateMap<Notification, NotificationDto>()
            .ForMember(d => d.Type, o => o.MapFrom(s => s.Type.ToString()));
        CreateMap<CreateNotificationRequest, Notification>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.IsRead, o => o.MapFrom(_ => false))
            .ForMember(d => d.CreatedAt, o => o.Ignore());
    }
}
