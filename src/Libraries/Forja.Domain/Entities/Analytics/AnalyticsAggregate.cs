namespace Forja.Domain.Entities.Analytics;

/// <summary>
/// Represents an aggregate of analytics data.
/// </summary>
[Table("AnalyticsAggregates", Schema = "analytics")]
public class AnalyticsAggregate
{
    /// <summary>
    /// Gets or sets the unique identifier for the analytics aggregate.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the date of the analytics data.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Gets or sets the name of the metric.
    /// </summary>
    [Required]
    public string MetricName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value of the metric.
    /// </summary>
    public decimal Value { get; set; }
}