using Microsoft.EntityFrameworkCore;
using Metrics.Utils.Enums;

namespace Metrics.Models;

public class MetricsContext : DbContext
{
    public MetricsContext(DbContextOptions<MetricsContext> options) : base(options)
    {
    }

    public DbSet<Metric> Metrics { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Metric>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.MetricName).IsRequired();
            entity.Property(e => e.MetricValue).IsRequired();
        });
    }

    public async Task<MetricResponse> GetMetricsAsync(MetricRequest request)
    {
        var query = Metrics.AsQueryable();

        // Filter by Id if provided
        if (request.Id.HasValue)
        {
            query = query.Where(m => m.Id == request.Id.Value);
        }

        // Filter by ClientId (required)
        query = query.Where(m => m.ClientId == request.ClientId);

        // Filter by FundId if provided
        if (request.FundId.HasValue)
        {
            query = query.Where(m => m.FundId == request.FundId.Value);
        }

        // Filter by date range
        if (request.StartDate.HasValue)
        {
            query = query.Where(m => m.AsOfDate >= request.StartDate.Value);
        }

        if (request.EndDate.HasValue)
        {
            query = query.Where(m => m.AsOfDate <= request.EndDate.Value);
        }

        // Handle aggregation
        if (request.Aggregation.HasValue)
        {
            if (request.Aggregation.Value == AggregationType.SUM)
            {
                var sum = await query.SumAsync(m => m.MetricValue);
                return new MetricResponse
                {
                    IsAggregation = true,
                    AggregationType = "SUM",
                    AggregationValue = sum
                };
            }
            else if (request.Aggregation.Value == AggregationType.COUNT)
            {
                var count = await query.CountAsync();
                return new MetricResponse
                {
                    IsAggregation = true,
                    AggregationType = "COUNT",
                    AggregationValue = count
                };
            }
        }

        // Apply pagination
        var results = await query
            .OrderBy(m => m.Id)
            .Skip(request.Offset)
            .Take(request.Limit)
            .ToListAsync();

        return new MetricResponse
        {
            IsAggregation = false,
            Metrics = results
        };
    }
}
