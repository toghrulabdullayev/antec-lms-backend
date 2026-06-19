using System.Text.Json.Serialization;

namespace AntecLMS.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AttendanceStatus
{
    [JsonPropertyName("present")]
    Present,

    [JsonPropertyName("late")]
    Late,

    [JsonPropertyName("absent_excused")]
    AbsentExcused,

    [JsonPropertyName("absent_unexcused")]
    AbsentUnexcused,
}
