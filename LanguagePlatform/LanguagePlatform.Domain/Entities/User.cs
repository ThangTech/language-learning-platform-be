using LanguagePlatform.Domain.Common;
using LanguagePlatform.Domain.Enums;

namespace LanguagePlatform.Domain.Entities;

public class User : AuditableEntity
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.User;
    public UserStatus Status { get; set; } = UserStatus.Active;
    public string? AvatarUrl { get; set; }
    // Trình độ của người học: Beginner, Intermediate, Advanced
    public string Level { get; set; } = "Beginner";

    // Navigation
    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    public ICollection<Flashcard> Flashcards { get; set; } = new List<Flashcard>();
    public ICollection<UserGrammar> UserGrammars { get; set; } = new List<UserGrammar>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public UserProgress? Progress { get; set; }
}
