using System.Text.Json.Serialization;

namespace Metrics.Utils.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AggregationType
{
    SUM,
    COUNT
}
