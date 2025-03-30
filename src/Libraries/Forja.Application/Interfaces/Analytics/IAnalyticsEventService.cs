namespace Forja.Application.Interfaces.Analytics;

/// <summary>
/// Represents a service for managing analytics events.
/// </summary>
public interface IAnalyticsEventService
{
    /// <summary>
    /// Retrieves an analytics event by its unique identifier.
    /// </summary>
    /// <param name="eventId">The unique identifier of the analytics event to retrieve.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the analytics event as a <see cref="AnalyticsEventDto"/>, or null if no event is found with the specified identifier.</returns>
    Task<AnalyticsEventDto?> GetByIdAsync(Guid eventId);

    /// <summary>
    /// Retrieves all analytics events asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, containing a collection of <see cref="AnalyticsEventDto"/> that represents all analytics events.</returns>
    Task<IEnumerable<AnalyticsEventDto>> GetAllAsync();

    /// <summary>
    /// Retrieves a collection of analytics events associated with a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose analytics events are being retrieved.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see cref="AnalyticsEventDto"/> associated with the specified user.</returns>
    Task<IEnumerable<AnalyticsEventDto>> GetByUserIdAsync(Guid userId);

    /// <summary>
    /// Adds a new analytics event based on the specified type, user, and associated metadata.
    /// </summary>
    /// <param name="eventType">The type of the analytics event to add, represented as <see cref="AnalyticEventType"/>.</param>
    /// <param name="userId">The unique identifier of the user associated with the event. Can be null if there is no associated user.</param>
    /// <param name="metadata">A dictionary containing additional metadata or attributes associated with the analytics event.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the added analytics event as a <see cref="AnalyticsEventDto"/>.</returns>
    Task<AnalyticsEventDto?> AddEventAsync(AnalyticEventType eventType,
       Guid? userId,
       Dictionary<string, string> metadata);

    /// <summary>
    /// Updates an existing analytics event with the provided details.
    /// </summary>
    /// <param name="request">An instance of <see cref="AnalyticsEventUpdateRequest"/> containing the updated analytics event details.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the updated analytics event as a <see cref="AnalyticsEventDto"/>, or null if the update operation fails or the event does not exist.</returns>
    Task<AnalyticsEventDto?> UpdateEventAsync(AnalyticsEventUpdateRequest request);

    /// <summary>
    /// Deletes an analytics event with the specified identifier.
    /// </summary>
    /// <param name="eventId">The unique identifier of the analytics event to be deleted.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteEventAsync(Guid eventId);
}