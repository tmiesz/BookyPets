using BookyPets.Contracts.Books;

namespace BookyPets.Contracts.Pets;

public record CreatePetRequest(string Name, Genre? FavouriteGenre);
