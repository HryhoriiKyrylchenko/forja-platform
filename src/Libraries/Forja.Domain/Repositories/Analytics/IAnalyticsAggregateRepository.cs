namespace Forja.Domain.Repositories.Analytics;

/// <summary>
/// Interface for managing AnalyticsAggregates data.
/// </summary>
public interface IAnalyticsAggregateRepository
{
    /// <summary>
    /// Gets an analytics aggregate by its ID.
    /// </summary>
    /// <param name="id">The ID of the analytics aggregate.</param>
    /// <returns>The analytics aggregate if found, null otherwise.</returns>
    Task<AnalyticsAggregate?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves all analytics aggregates, optionally filtered by a date range.
    /// </summary>
    /// <param name="startDate">The start date for filtering the analytics aggregates. If null, no start date filter is applied.</param>
    /// <param name="endDate">The end date for filtering the analytics aggregates. If null, no end date filter is applied.</param>
    /// <returns>A collection of analytics aggregates matching the specified criteria.</returns>
    Task<IEnumerable<AnalyticsAggregate>> GetAllAsync(DateTime? startDate = null, DateTime? endDate = null);

    /// <summary>
    /// Adds a new analytics aggregate to the database.
    /// </summary>
    /// <param name="aggregate">The analytics aggregate to add.</param>
    Task<AnalyticsAggregate?> AddAsync(AnalyticsAggregate aggregate);

    /// <summary>
    /// Updates an existing analytics aggregate.
    /// </summary>
    /// <param name="aggregate">The analytics aggregate to update.</param>
    Task<AnalyticsAggregate?> UpdateAsync(AnalyticsAggregate aggregate);

    /// <summary>
    /// Deletes an analytics aggregate by its ID.
    /// </summary>
    /// <param name="id">The ID of the analytics aggregate to delete.</param>
    Task DeleteAsync(Guid id);
}