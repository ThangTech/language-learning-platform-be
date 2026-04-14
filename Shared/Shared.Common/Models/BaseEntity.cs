namespace Shared.Common.Models;

/// <summary>
/// Base entity with common properties for all database entities.
/// </summary>
public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Base entity with audit tracking (CreatedAt + UpdatedAt).
/// </summary>
public abstract class AuditableEntity : BaseEntity
{
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
