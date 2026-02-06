namespace Metrics.Models;

public class Metric
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public int? FundId { get; set; }
    public DateTime AsOfDate { get; set; }
    public string MetricName { get; set; } = string.Empty;
    public decimal MetricValue { get; set; }
}
