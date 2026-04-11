using System.Text.Json.Serialization;

namespace BookyPets.Contracts.Books;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Genre
{
    Science,
    Technology,
    Philosophy,
    History,
    Psychology,
    Fiction,
    Fantasy,
    Biography,
    Educational
}
