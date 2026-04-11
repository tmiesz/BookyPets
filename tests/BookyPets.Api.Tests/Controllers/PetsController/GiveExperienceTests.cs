using System.Net;
using System.Net.Http.Json;
using BookyPets.Api.Tests.Common;
using BookyPets.Contracts.Pets;
using BookyPets.Domain.Tests.TestConstants;

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
    public async Task GiveExperience_WhenValidPet_ShouldReturnNoContent()
    {
        var createResponse = await _client.PostAsJsonAsync("Pets", new CreatePetRequest(Constants.Pet.Name, Constants.Pet.FavouriteContractsGenre));
        var pet = await createResponse.Content.ReadFromJsonAsync<PetResponse>();

        var response = await _client.PostAsync($"Pets/{pet!.Id}/experience/100", null);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task GiveExperience_WhenPetDoesNotExist_ShouldReturnNotFound()
    {
        var response = await _client.PostAsync($"Pets/{Guid.NewGuid()}/experience/100", null);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
