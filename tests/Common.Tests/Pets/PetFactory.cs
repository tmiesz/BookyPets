using BookyPets.Domain.BookAggregate;
using BookyPets.Domain.PetAggregate;
using BookyPets.Domain.Tests.TestConstants;

namespace Common.Tests.Pets;

public static class PetFactory
{
    public static Pet CreatePet(
        string? name = null,
        Species? species = null,
        Genre? genre = null,
        Guid? id = null)
    {
        return new Pet(
            name: name ?? Constants.Pet.Name,
            species: species ?? Constants.Pet.Species,
            favouriteGenre: genre ?? Constants.Pet.FavouriteGenre,
            id: id ?? Constants.Pet.Id);
    }
}
