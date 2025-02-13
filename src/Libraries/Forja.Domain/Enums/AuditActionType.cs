namespace Forja.Domain.Enums;

/// <summary>
/// Represents the types of actions that can be audited.
/// </summary>
public enum AuditActionType
{
    /// <summary>
    /// Indicates that an entity was created.
    /// </summary>
    Create,

    /// <summary>
    /// Indicates that an entity was updated.
    /// </summary>
    Update,

    /// <summary>
    /// Indicates that an entity was deleted.
    /// </summary>
    Delete,
}