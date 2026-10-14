using System.Net;
using BookyPets.Api.Tests.Common;
using BookyPets.Contracts.Pets;

namespace BookyPets.Api.Tests.Controllers.PetsController;

[Collection(BookyPetsApiFactoryCollection.CollectionName)]
public class SearchPetsTests
{
    private readonly HttpClient _client;

    public SearchPetsTests(BookyPetsApiFactory apiFactory)
    {
        _client = apiFactory.HttpClient;

        apiFactory.ResetDatabase();
    }

    private async Task SeedPetsAsync()
    {
        await _client.CreatePetAsync(name: "Gamma");
        await _client.CreatePetAsync(name: "Alpha");
        await _client.CreatePetAsync(name: "Beta");
    }

    [Fact]
    public async Task SearchPets_WhenLimitIsGiven_ShouldReturnAtMostThatManyPets()
    {
        await SeedPetsAsync();

        var response = await _client.GetAsync("Pets?limit=2");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var pets = await response.ReadRequiredAsync<PetResponse[]>();
        Assert.Equal(2, pets.Length);
    }

    [Fact]
    public async Task SearchPets_WhenSearchTextIsGiven_ShouldReturnMatchingPetsOnly()
    {
        await SeedPetsAsync();

        var response = await _client.GetAsync("Pets?search=beta");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var pets = await response.ReadRequiredAsync<PetResponse[]>();
        Assert.Equal("Beta", Assert.Single(pets).Name);
    }

    [Theory]
    [InlineData("limit=0", "Limit")]
    [InlineData("limit=101", "Limit")]
    [InlineData("offset=-1", "Offset")]
    public async Task SearchPets_WhenPagingIsOutOfRange_ShouldReturnBadRequestWithTheFailingProperty(
        string query,
        string expectedCode)
    {
        var response = await _client.GetAsync($"Pets?{query}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal(expectedCode, await response.ReadProblemCodeAsync());
    }

    [Fact]
    public async Task SearchPets_WhenOffsetSkipsAllPets_ShouldReturnAnEmptyList()
    {
        await SeedPetsAsync();

        var response = await _client.GetAsync("Pets?offset=3");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Empty(await response.ReadRequiredAsync<PetResponse[]>());
    }
}
