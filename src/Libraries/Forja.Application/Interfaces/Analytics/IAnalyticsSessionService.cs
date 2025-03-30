namespace Forja.Application.Interfaces.Analytics;

/// <summary>
/// Defines the methods for managing and interacting with analytics sessions.
/// </summary>
public interface IAnalyticsSessionService
{
    /// <summary>
    /// Retrieves an analytics session by its unique identifier.
    /// </summary>
    /// <param name="sessionId">The unique identifier of the analytics session to retrieve.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the <see cref="AnalyticsSessionDto"/>
    /// representing the analytics session if found; otherwise, null.</returns>
    Task<AnalyticsSessionDto?> GetByIdAsync(Guid sessionId);

    /// <summary>
    /// Retrieves all analytics sessions.
    /// </summary>
    /// <returns>
    /// A task that represents an asynchronous operation. The task result contains a collection of
    /// AnalyticsSessionDto representing all analytics sessions.
    /// </returns>
    Task<IEnumerable<AnalyticsSessionDto>> GetAllAsync();

    /// <summary>
    /// Retrieves a collection of analytics sessions associated with the specified user ID.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose analytics sessions are to be retrieved.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see cref="AnalyticsSessionDto"/> objects
    /// associated with the specified user ID.
    /// </returns>
    Task<IEnumerable<AnalyticsSessionDto>> GetByUserIdAsync(Guid userId);

    /// <summary>
    /// Adds a new analytics session.
    /// </summary>
    /// <param name="userId">The unique identifier of the user associated with the session. Can be null if the session is not tied to a specific user.</param>
    /// <param name="metadata">A dictionary containing metadata associated with the analytics session.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the <see cref="AnalyticsSessionDto"/>
    /// representing the added analytics session.</returns>
    Task<AnalyticsSessionDto?> AddSessionAsync(Guid? userId, Dictionary<string, string> metadata);

    /// <summary>
    /// Marks the specified analytics session as ended by updating the session's end time.
    /// </summary>
    /// <param name="sessionId">The unique identifier of the analytics session to end.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task EndSessionAsync(Guid sessionId);

    /// <summary>
    /// Deletes an analytics session associated with the specified session ID.
    /// </summary>
    /// <param name="sessionId">The unique identifier of the analytics session to be deleted.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteSessionAsync(Guid sessionId);
}