namespace Forja.Application.DTOs.Analytics;

public class AnalyticsAggregateDto
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string MetricName { get; set; } = string.Empty;
    public decimal Value { get; set; }
}