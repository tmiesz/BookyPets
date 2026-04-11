using System.Text.Json.Serialization;

namespace BookyPets.Contracts.Readers;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AccountType
{
    Free,
    Basic,
    Pro
}
