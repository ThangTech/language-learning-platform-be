using FluentValidation;
using LanguagePlatform.Application.Interfaces;
using LanguagePlatform.Application.Mappings;
using LanguagePlatform.Application.Services;
using LanguagePlatform.Application.Validators;
using LanguagePlatform.Domain.Interfaces;
using LanguagePlatform.Infrastructure.Persistence;
using LanguagePlatform.Infrastructure.Repositories;
using LanguagePlatform.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LanguagePlatform.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        // Database
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IWordRepository, WordRepository>();
        services.AddScoped<IFavoriteRepository, FavoriteRepository>();
        services.AddScoped<IFlashcardRepository, FlashcardRepository>();
        services.AddScoped<IGrammarRepository, GrammarRepository>();
        services.AddScoped<IUserGrammarRepository, UserGrammarRepository>();
        services.AddScoped<IListeningRepository, ListeningRepository>();
        services.AddScoped<IListeningResultRepository, ListeningResultRepository>();
        services.AddScoped<IDictationSetRepository, DictationSetRepository>();
        services.AddScoped<IQuizRepository, QuizRepository>();
        services.AddScoped<IQuizResultRepository, QuizResultRepository>();
        services.AddScoped<IProgressRepository, ProgressRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();

        // Infrastructure services
        services.AddScoped<IJwtService, JwtService>();

        return services;
    }

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(cfg => cfg.AddMaps(typeof(MappingProfile).Assembly));

        // FluentValidation - scan all validators in Application assembly
        services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();

        // Application services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserAdminService, UserAdminService>();
        services.AddScoped<IVocabularyService, VocabularyService>();
        services.AddScoped<IGrammarService, GrammarService>();
        services.AddScoped<IListeningService, ListeningService>();
        services.AddScoped<IQuizService, QuizService>();
        services.AddScoped<IProgressService, ProgressService>();
        services.AddScoped<INotificationService, NotificationService>();

        return services;
    }
}
