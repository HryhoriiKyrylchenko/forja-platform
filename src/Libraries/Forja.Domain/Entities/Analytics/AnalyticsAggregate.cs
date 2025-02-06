namespace Forja.Domain.Entities.Analytics;

[Table("AnalyticsAggregates", Schema = "analytics")]
public class AnalyticsAggregate
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public DateTime Date { get; set; }

    [Required]
    public string MetricName { get; set; } = string.Empty;

    public decimal Value { get; set; }
}