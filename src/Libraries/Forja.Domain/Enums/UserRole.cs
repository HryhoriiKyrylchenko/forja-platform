namespace Forja.Domain.Enums;

/// <summary>
/// Defines various roles assigned to users within the system,
/// each with different levels of access and responsibilities.
/// </summary>
public enum UserRole
{
    /// <summary>
    /// Represents a user role with standard access permissions,
    /// typically assigned to a general user of the system.
    /// </summary>
    User,

    /// <summary>
    /// Represents a role with the highest level of access and privileges
    /// within the system, typically responsible for overseeing and managing
    /// all aspects of the platform.
    /// </summary>
    Administrator,

    /// <summary>
    /// Represents a user role responsible for managing and organizing content within the system,
    /// including creation, editing, and publishing of digital resources.
    /// </summary>
    ContentManager,

    /// <summary>
    /// Represents a user role responsible for managing sales processes,
    /// typically overseeing sales operations and strategy within the system.
    /// </summary>
    SalesManager,

    /// <summary>
    /// Represents a user role responsible for managing support-related tasks,
    /// ensuring customer inquiries and issues are addressed efficiently.
    /// </summary>
    SupportManager,

    /// <summary>
    /// Represents a user role responsible for analyzing data
    /// and providing insights to support decision-making processes.
    /// </summary>
    AnalyticsManager,

    /// <summary>
    /// Represents a user role with permissions to oversee and manage user-generated content,
    /// ensuring community guidelines and policies are upheld.
    /// </summary>
    Moderator,

    /// <summary>
    /// Denotes a role with the highest level of system access and control.
    /// Typically responsible for managing system settings, configurations, and user permissions.
    /// </summary>
    SystemAdministrator
}