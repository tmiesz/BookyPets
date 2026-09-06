using System.Text.Json.Serialization;

namespace BookyPets.Contracts.Pets;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Species
{
    Bear,
    Bee,
    Bird,
    Butterfly,
    Dog,
    Lion,
    Tiger,
    Whale
}
