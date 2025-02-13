namespace Forja.Domain.Entities.Common;

/// <summary>
/// Represents an entity that can be soft deleted.
/// </summary>
public abstract class SoftDeletableEntity
{
    /// <summary>
    /// Gets or sets a value indicating whether the entity is deleted.
    /// </summary>
    public bool IsDeleted { get; set; } = false;
}