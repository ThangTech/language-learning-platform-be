using LanguagePlatform.Domain.Common;

namespace LanguagePlatform.Domain.Entities;

public class UserGrammar : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid TopicId { get; set; }
    public bool IsCompleted { get; set; } = false;
    public DateTime? CompletedAt { get; set; }

    // Navigation
    public User User { get; set; } = null!;
    public GrammarTopic Topic { get; set; } = null!;
}
