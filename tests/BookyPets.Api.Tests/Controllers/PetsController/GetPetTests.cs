using System.Net;
using System.Net.Http.Json;
using BookyPets.Api.Tests.Common;
using BookyPets.Contracts.Pets;
using BookyPets.Domain.Tests.TestConstants;

namespace BookyPets.Api.Tests.Controllers.PetsController;

[Collection(BookyPetsApiFactoryCollection.CollectionName)]
public class GetPetTests
{
    private readonly HttpClient _client;

    public GetPetTests(BookyPetsApiFactory apiFactory)
    {
        _client = apiFactory.HttpClient;

        apiFactory.ResetDatabase();
    }

    [Fact]
    public async Task GetPet_WhenPetExists_ShouldReturnPet()
    {
        var createResponse = await _client.PostAsJsonAsync("Pets", new CreatePetRequest(Constants.Pet.Name, Constants.Pet.FavouriteContractsGenre));
        var created = await createResponse.Content.ReadFromJsonAsync<PetResponse>();

        var response = await _client.GetAsync($"Pets/{created!.Id}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var pet = await response.Content.ReadFromJsonAsync<PetResponse>();
        Assert.NotNull(pet);
        Assert.Equal(Constants.Pet.Name, pet.Name);
    }

    [Fact]
    public async Task GetPet_WhenPetDoesNotExist_ShouldReturnNotFound()
    {
        var response = await _client.GetAsync($"Pets/{Guid.NewGuid()}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
