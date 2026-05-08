using LanguagePlatform.Domain.Common;

namespace LanguagePlatform.Domain.Entities;

public class Flashcard : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid WordId { get; set; }
    public bool IsLearned { get; set; } = false;
    public int ReviewCount { get; set; } = 0;
    public DateTime? NextReviewAt { get; set; }
    public DateTime? LearnedAt { get; set; }

    // Navigation
    public User User { get; set; } = null!;
    public Word Word { get; set; } = null!;
}
