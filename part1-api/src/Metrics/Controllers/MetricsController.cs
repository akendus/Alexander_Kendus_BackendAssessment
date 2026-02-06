using Microsoft.AspNetCore.Mvc;
using Metrics.Models;

namespace Metrics.Controllers;

[ApiController]
[Route("[controller]")]
public class MetricsController : ControllerBase
{
    private readonly MetricsContext _context;

    public MetricsController(MetricsContext context)
    {
        _context = context;
    }

    [HttpGet]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromQuery, FromRoute] MetricRequest request)
    {
        var result = await _context.GetMetricsAsync(request);

        if (!result.IsAggregation && (result.Metrics == null || result.Metrics.Count == 0))
        {
            return NotFound();
        }

        return Ok(result);
    }
}
