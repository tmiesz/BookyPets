using System.Text.Json.Serialization;

namespace BookyPets.Contracts.Sessions;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SessionStatus
{
    Active,
    Completed,
    Dropped,
}
