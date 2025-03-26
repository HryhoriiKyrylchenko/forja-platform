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
    /// Gets all analytics aggregates.
    /// </summary>
    /// <returns>A list of all analytics aggregates.</returns>
    Task<IEnumerable<AnalyticsAggregate>> GetAllAsync();

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