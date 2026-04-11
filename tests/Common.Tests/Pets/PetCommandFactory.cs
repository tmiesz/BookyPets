using BookyPets.Application.Pets.Commands.CreatePet;
using BookyPets.Application.Pets.Queries.GetPet;
using BookyPets.Domain.BookAggregate;
using BookyPets.Domain.Tests.TestConstants;

namespace Common.Tests.Pets;

public static class PetCommandFactory
{
    public static CreatePetCommand CreateCreatePetCommand(
        string? name = null,
        Genre? genre = null)
    {
        return new CreatePetCommand(
            Name: name ?? Constants.Pet.Name,
            Genre: genre ?? Constants.Pet.FavouriteGenre);
    }

    public static GetPetQuery CreateGetPetQuery(
        Guid? petId = null)
    {
        return new GetPetQuery(
            PetId: petId ?? Constants.Pet.Id);
    }
}
