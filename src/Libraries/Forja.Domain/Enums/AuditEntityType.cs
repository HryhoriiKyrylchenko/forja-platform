namespace Forja.Domain.Enums;

/// <summary>
/// Represents the type of entity being audited.
/// </summary>
public enum AuditEntityType
{
    /// <summary>
    /// Represents a game entity.
    /// </summary>
    Game,

    /// <summary>
    /// Represents a game addon entity.
    /// </summary>
    GameAddon,

    /// <summary>
    /// Represents a bundle entity.
    /// </summary>
    Bundle,
}