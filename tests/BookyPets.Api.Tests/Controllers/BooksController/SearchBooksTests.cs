using System.Net;
using BookyPets.Api.Tests.Common;

namespace BookyPets.Api.Tests.Controllers.BooksController;

[Collection(BookyPetsApiFactoryCollection.CollectionName)]
public class SearchBooksTests
{
    private readonly HttpClient _client;

    public SearchBooksTests(BookyPetsApiFactory apiFactory)
    {
        _client = apiFactory.HttpClient;

        apiFactory.ResetDatabase();
    }

    private async Task SeedBooksAsync()
    {
        await _client.CreateBookAsync(title: "Gamma Book");
        await _client.CreateBookAsync(title: "Alpha Book");
        await _client.CreateBookAsync(title: "Beta Book");
    }

    [Fact]
    public async Task SearchBooks_WhenLimitIsGiven_ShouldReturnAtMostThatManyBooks()
    {
        await SeedBooksAsync();

        var response = await _client.GetAsync("Books?limit=2");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task SearchBooks_WhenSearchTextIsGiven_ShouldReturnMatchingBooksOnly()
    {
        await SeedBooksAsync();

        var response = await _client.GetAsync("Books?search=gamma");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(51)]
    public async Task SearchBooks_WhenLimitIsOutOfRange_ShouldReturnBadRequestWithTheFailingProperty(int limit)
    {
        var response = await _client.GetAsync($"Books?limit={limit}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(50)]
    public async Task SearchBooks_WhenLimitIsAtTheBoundary_ShouldSucceed(int limit)
    {
        var response = await _client.GetAsync($"Books?limit={limit}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
