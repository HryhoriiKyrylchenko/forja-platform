namespace Forja.Application.Interfaces.Analytics;

public interface IAnalyticsAggregateService
{
    /// <summary>
    /// Retrieves a list of analytics aggregates for sessions within the specified date range.
    /// </summary>
    /// <param name="startDate">
    /// The start date to filter the sessions.
    /// </param>
    /// <param name="endDate">
    /// The optional end date to filter the sessions. If null, the filter is not applied.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of
    /// <see cref="AnalyticsAggregateDto"/> representing the analytics aggregates for the specified date range.
    /// </returns>
    Task<List<AnalyticsAggregateDto>> GetAnalyticsAggregateOfSessionsAsync(DateTime startDate, DateTime? endDate = null);

    /// <summary>
    /// Retrieves aggregated analytics data for events within an optional time range and filtered by a specific event type.
    /// </summary>
    /// <param name="startDate">The start date of the range to aggregate events from.</param>
    /// <param name="endDate">The end date of the range to aggregate events to. Default is null.</param>
    /// <param name="eventType">The type of event to filter the aggregation by. Default is null.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a list of analytics aggregate data.</returns>
    Task<List<AnalyticsAggregateDto>> GetAnalyticsAggregateOfEventsAsync(AnalyticEventType eventType, DateTime startDate, 
        DateTime? endDate = null);
}