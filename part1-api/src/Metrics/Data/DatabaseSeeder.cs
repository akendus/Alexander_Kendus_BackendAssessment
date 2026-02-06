using System.Runtime.Serialization;
using Metrics.Models;
using Metrics.Utils.Enums;

namespace Metrics.Data;

public static class DatabaseSeeder
{
    public static void SeedDatabase(MetricsContext context)
    {
        var random = new Random();
        var metricNames = Enum.GetValues<MetricName>();

        for (int i = 1; i <= 1000; i++)
        {
            var metricName = metricNames[random.Next(metricNames.Length)];
            var metricNameValue = metricName.GetType()
                .GetField(metricName.ToString())
                ?.GetCustomAttributes(typeof(EnumMemberAttribute), false)
                .Cast<EnumMemberAttribute>()
                .FirstOrDefault()?.Value ?? metricName.ToString();

            context.Metrics.Add(new Metric
            {
                Id = i,
                ClientId = random.Next(100, 111), // 100 to 110 inclusive
                FundId = random.Next(0, 6), // 0 to 5 inclusive
                AsOfDate = DateTime.Now.AddDays(-random.Next(0, 365)),
                MetricName = metricNameValue,
                MetricValue = Math.Round((decimal)(random.NextDouble() * 1000), 2)
            });
        }

        context.SaveChanges();
    }
}
