using System.Net;
using System.Net.Http.Json;
using BookyPets.Api.Tests.Common;
using BookyPets.Api.Tests.Common.Pets;
using BookyPets.Contracts.Pets;
using BookyPets.Domain.Tests.TestConstants;

namespace BookyPets.Api.Tests.Controllers.PetsController;

[Collection(BookyPetsApiFactoryCollection.CollectionName)]
public class CreatePetTests : PetSpecies
{
    private readonly HttpClient _client;

    public CreatePetTests(BookyPetsApiFactory apiFactory)
    {
        _client = apiFactory.HttpClient;

        apiFactory.ResetDatabase();
    }

    [Theory]
    [MemberData(nameof(ListSpecies))]
    public async Task CreatePet_WhenValidPet_ShouldCreatePet(Species species)
    {
        var createPetRequest = new CreatePetRequest(
            Constants.Pet.Name,
            species,
            Constants.Pet.FavouriteContractsGenre);

        var response = await _client.PostAsJsonAsync("Pets", createPetRequest);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var petResponse = await response.Content.ReadFromJsonAsync<PetResponse>();
        Assert.NotNull(petResponse);
        Assert.Equal(Constants.Pet.Name, petResponse.Name);
        Assert.Equal(species, petResponse.Species);
        Assert.Equal(Constants.Pet.FavouriteContractsGenre, petResponse.FavouriteGenre);
    }
}
