using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Metrics.Utils.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MetricName
{
    [EnumMember(Value = "marketValue")]
    MarketValue,

    [EnumMember(Value = "marketNotion")]
    MarketNotion
}
