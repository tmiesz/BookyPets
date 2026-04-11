using BookyPets.Contracts.Books;

namespace BookyPets.Contracts.Pets;

public record PetResponse(Guid Id, string Name, Genre? FavouriteGenre, int Level);
