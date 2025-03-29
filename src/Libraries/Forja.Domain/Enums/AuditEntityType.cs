namespace Forja.Domain.Enums;

/// <summary>
/// Represents the type of entity being audited.
/// </summary>
public enum AuditEntityType
{
    User,
    Product,
    Order,
    Payment,
    Other
}