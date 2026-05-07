using LanguagePlatform.Domain.Common;

namespace LanguagePlatform.Domain.Entities;

public class Favorite : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid WordId { get; set; }

    // Navigation
    public User User { get; set; } = null!;
    public Word Word { get; set; } = null!;
}
