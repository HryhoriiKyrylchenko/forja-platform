namespace Forja.Domain.Enums;

/// <summary>
/// Represents the types of actions that can be audited.
/// </summary>
public enum AuditActionType
{
    Create,
    Update,
    Delete,
    View,
    Login,
    Logout,
    UnauthorizedAccess,
    SystemTriggeredEvent,
    
    ValidationError,
    SystemError,
    DatabaseError,
    ApiError,
    AuthenticationError,
    BusinessLogicError,
    Miscellaneous
}