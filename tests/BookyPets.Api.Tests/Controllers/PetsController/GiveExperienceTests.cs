using System.Net;
using System.Net.Http.Json;
using BookyPets.Api.Tests.Common;
using BookyPets.Contracts.Pets;
using Common.Tests.TestConstants;

namespace BookyPets.Api.Tests.Controllers.PetsController;

[Collection(BookyPetsApiFactoryCollection.CollectionName)]
public class GiveExperienceTests
{
    private readonly HttpClient _client;

    public GiveExperienceTests(BookyPetsApiFactory apiFactory)
    {
        _client = apiFactory.HttpClient;

        apiFactory.ResetDatabase();
    }

    [Fact]
    public async Task GiveExperience_WhenValidPet_ShouldReturnNoContentAndUpdateThePet()
    {
        var createResponse = await _client.PostAsJsonAsync("Pets", new CreatePetRequest(Constants.Pet.Name, Constants.Pet.ContractSpecies, Constants.Pet.FavouriteContractsGenre));
        var pet = await createResponse.Content.ReadFromJsonAsync<PetResponse>();

        var response = await _client.PostAsync($"Pets/{pet!.Id}/experience/150", null);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var getResponse = await _client.GetAsync($"Pets/{pet.Id}");
        var updated = await getResponse.ReadRequiredAsync<PetResponse>();

        Assert.Equal(2, updated.Level);
        Assert.Equal(50, updated.Experience);
        Assert.Equal(200, updated.ExperienceToNextLevel);
    }

    [Fact]
    public async Task GiveExperience_WhenExperienceIsNegative_ShouldReturnBadRequestAndKeepThePetUnchanged()
    {
        var pet = await _client.CreatePetAsync();

        var response = await _client.PostAsync($"Pets/{pet.Id}/experience/-5", null);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("Experience", await response.ReadProblemCodeAsync());

        var getResponse = await _client.GetAsync($"Pets/{pet.Id}");
        var unchanged = await getResponse.ReadRequiredAsync<PetResponse>();
        Assert.Equal(1, unchanged.Level);
        Assert.Equal(0, unchanged.Experience);
    }

    [Fact]
    public async Task GiveExperience_WhenPetDoesNotExist_ShouldReturnNotFound()
    {
        var response = await _client.PostAsync($"Pets/{Guid.NewGuid()}/experience/100", null);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
