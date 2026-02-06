using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Metrics.Utils.Enums;

namespace Metrics.Models;

public class MetricRequest : IValidatableObject
{
    [FromRoute(Name = "id")]
    public int? Id { get; set; }

    [FromQuery(Name = "client_id")]
    public int ClientId { get; set; }

    [FromQuery(Name = "fund_id")]
    public int? FundId { get; set; }

    [FromQuery(Name = "start_date")]
    public DateTime? StartDate { get; set; }

    [FromQuery(Name = "end_date")]
    public DateTime? EndDate { get; set; }

    [FromQuery(Name = "aggregation")]
    public AggregationType? Aggregation { get; set; }

    [FromQuery(Name = "limit")]
    [Range(1, 1000, ErrorMessage = "Limit must be between 1 and 1000")]
    public int Limit { get; set; } = 100;

    [FromQuery(Name = "offset")]
    [Range(0, int.MaxValue, ErrorMessage = "Offset must be greater than or equal to 0")]
    public int Offset { get; set; } = 0;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (StartDate.HasValue && EndDate.HasValue && EndDate < StartDate)
        {
            yield return new ValidationResult(
                "End date must be greater than or equal to start date",
                new[] { nameof(EndDate) });
        }
    }
}
