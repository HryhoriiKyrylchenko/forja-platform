namespace Forja.Domain.Repositories.Analytics;

/// <summary>
/// Interface to manage AnalyticsSessions data.
/// </summary>
public interface IAnalyticsSessionRepository
{
    /// <summary>
    /// Retrieves an analytics session by its session ID.
    /// </summary>
    /// <param name="sessionId">The session ID.</param>
    /// <returns>The analytics session if found, null otherwise.</returns>
    Task<AnalyticsSession?> GetByIdAsync(Guid sessionId);

    /// <summary>
    /// Retrieves all analytics sessions, optionally filtered by start and end dates.
    /// </summary>
    /// <param name="startDate">The optional start date to filter sessions. If null, no filtering is applied.</param>
    /// <param name="endDate">The optional end date to filter sessions. If null, no filtering is applied.</param>
    /// <returns>A list of analytics sessions matching the specified criteria.</returns>
    Task<IEnumerable<AnalyticsSession>> GetAllAsync(DateTime? startDate = null, DateTime? endDate = null);


    /// <summary>
    /// Retrieves the count of analytics sessions for a specific date.
    /// </summary>
    /// <param name="date">The date for which to count the analytic sessions.</param>
    /// <returns>The total number of analytics sessions on the specified date.</returns>
    Task<int> GetSessionCountAsync(DateTime date);

    /// <summary>
    /// Retrieves analytics sessions by user ID.
    /// </summary>
    /// <param name="userId">The user ID associated with the sessions.</param>
    /// <returns>A list of analytics sessions for the specified user.</returns>
    Task<IEnumerable<AnalyticsSession>> GetByUserIdAsync(Guid userId);

    /// <summary>
    /// Adds a new analytics session to the database.
    /// </summary>
    /// <param name="analyticsSession">The analytics session to add.</param>
    Task<AnalyticsSession?> AddAsync(AnalyticsSession analyticsSession);

    /// <summary>
    /// Updates an existing analytics session.
    /// </summary>
    /// <param name="analyticsSession">The analytics session to update.</param>
    Task<AnalyticsSession?> UpdateAsync(AnalyticsSession analyticsSession);

    /// <summary>
    /// Deletes an analytics session by its session ID.
    /// </summary>
    /// <param name="sessionId">The session ID of the analytics session to delete.</param>
    Task DeleteAsync(Guid sessionId);
}