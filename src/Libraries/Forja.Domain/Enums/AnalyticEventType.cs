namespace Forja.Domain.Enums;

/// <summary>
/// Represents the types of analytic events that can be tracked.
/// </summary>
public enum AnalyticEventType
{
    /// <summary>
    /// Event type for tracking page views.
    /// </summary>
    PageView,

    /// <summary>
    /// Event type for tracking purchases.
    /// </summary>
    Purchase,

    /// <summary>
    /// Event type for tracking submitted reviews.
    /// </summary>
    ReviewSubmitted,
}