namespace Forja.Domain.Enums;

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