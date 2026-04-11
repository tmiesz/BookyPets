using System.Text.Json.Serialization;

namespace BookyPets.Contracts.Books;

public record CreateBookRequest(string Title, string Author, [property: JsonRequired] Genre Genre, int PageCount);
