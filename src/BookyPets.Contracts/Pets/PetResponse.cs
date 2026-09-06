using BookyPets.Contracts.Books;

namespace BookyPets.Contracts.Pets;

public record PetResponse(Guid Id, string Name, Species Species, string IconUrl, Genre? FavouriteGenre, int Level);
