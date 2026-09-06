using System.Text.Json.Serialization;
using BookyPets.Contracts.Books;

namespace BookyPets.Contracts.Pets;

public record CreatePetRequest(string Name, [property: JsonRequired] Species Species, Genre? FavouriteGenre);
