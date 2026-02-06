namespace Metrics.Models;

public class MetricResponse
{
    public bool IsAggregation { get; set; }
    public string? AggregationType { get; set; }
    public decimal? AggregationValue { get; set; }
    public List<Metric>? Metrics { get; set; }
}
