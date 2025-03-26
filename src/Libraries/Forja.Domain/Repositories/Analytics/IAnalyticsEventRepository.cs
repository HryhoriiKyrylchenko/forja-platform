namespace Forja.Domain.Repositories.Analytics;

/// <summary>
/// Interface for managing AnalyticsEvent data.
/// </summary>
public interface IAnalyticsEventRepository
{
    /// <summary>
    /// Retrieves an analytics event by its ID.
    /// </summary>
    /// <param name="eventId">The ID of the analytics event.</param>
    /// <returns>The analytics event if found, null otherwise.</returns>
    Task<AnalyticsEvent?> GetByIdAsync(Guid eventId);

    /// <summary>
    /// Retrieves all analytics events.
    /// </summary>
    /// <returns>A list of all analytics events.</returns>
    Task<IEnumerable<AnalyticsEvent>> GetAllAsync();

    /// <summary>
    /// Retrieves all analytics events for a specific user.
    /// </summary>
    /// <param name="userId">The user ID associated with the events.</param>
    /// <returns>A list of analytics events for the specified user.</returns>
    Task<IEnumerable<AnalyticsEvent>> GetByUserIdAsync(Guid userId);

    /// <summary>
    /// Adds a new analytics event to the database.
    /// </summary>
    /// <param name="analyticsEvent">The analytics event to add.</param>
    Task<AnalyticsEvent?> AddAsync(AnalyticsEvent analyticsEvent);

    /// <summary>
    /// Updates an existing analytics event.
    /// </summary>
    /// <param name="analyticsEvent">The analytics event to update.</param>
    Task<AnalyticsEvent?> UpdateAsync(AnalyticsEvent analyticsEvent);

    /// <summary>
    /// Deletes an analytics event by its ID.
    /// </summary>
    /// <param name="eventId">The ID of the analytics event to delete.</param>
    Task DeleteAsync(Guid eventId);
}