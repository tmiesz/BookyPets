using BookyPets.Contracts.Pets;

namespace BookyPets.Api.Tests.Common.Pets;

[Collection(BookyPetsApiFactoryCollection.CollectionName)]
public class PetSpecies
{
    public static TheoryData<Species> ListSpecies()
    {
        var speciesList = Enum.GetValues<Species>().ToList();

        var theoryData = new TheoryData<Species>();

        foreach (var species in speciesList)
        {
            theoryData.Add(species);
        }

        return theoryData;
    }
}
