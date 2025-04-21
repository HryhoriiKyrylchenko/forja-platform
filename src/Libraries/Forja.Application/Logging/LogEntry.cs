namespace Forja.Application.Logging;

/// <summary>
/// Represents a log entry used for auditing purposes.
/// </summary>
/// <typeparam name="TState">The type of the state being logged.</typeparam>
public class LogEntry<TState>
{
    /// <summary>
    /// Represents the current state or context associated with the log entry.
    /// </summary>
    /// <typeparam name="TState">
    /// Specifies the type of the state being logged, allowing flexibility to store states of various data types.
    /// </typeparam>
    public required TState State { get; set; }

    /// Represents the unique identifier for a user associated with the log entry.
    /// This property can be used to track which user is related to a particular logged action or event.
    /// It is optional and allows null values, which might be applicable when user information is not available.
    public Guid? UserId { get; set; }

    /// <summary>
    /// Gets or sets the exception associated with the log entry.
    /// </summary>
    /// <remarks>
    /// This property captures any exception that occurs during the execution of a specific operation
    /// and is recorded as part of the log entry.
    /// </remarks>
    public Exception? Exception { get; set; }

    /// <summary>
    /// Gets or sets the action type associated with the log entry.
    /// This specifies the type of action being logged, such as Create, Update, Delete, View, Login, Logout, or other audit-related actions.
    /// </summary>
    /// <remarks>
    /// This property is used to categorize and identify the nature of the action being audited or logged.
    /// The action type is represented as an <c>AuditActionType</c> enumeration.
    /// </remarks>
    public AuditActionType ActionType { get; set; }

    /// <summary>
    /// Represents the type of the entity being audited or logged.
    /// </summary>
    /// <remarks>
    /// The <c>EntityType</c> property identifies the specific category or classification
    /// of the entity involved in the operation being logged, such as User, Product, Order, etc.
    /// This assists in categorizing and filtering log entries for easier analysis.
    /// </remarks>
    public AuditEntityType EntityType { get; set; }

    /// <summary>
    /// Represents the severity level of a log associated with the log entry.
    /// </summary>
    /// <remarks>
    /// This property defines the importance or severity of the event being logged.
    /// Common log levels include Trace, Debug, Information, Warning, Error, and Critical.
    /// It helps categorize and filter log messages based on their significance.
    /// </remarks>
    public LogLevel LogLevel { get; set; }

    /// <summary>
    /// A dictionary containing additional information or contextual details
    /// related to the log entry. It stores key-value pairs where the key
    /// represents the description or category, and the value provides
    /// the corresponding information.
    /// </summary>
    public Dictionary<string, string>? Details { get; set; }
}