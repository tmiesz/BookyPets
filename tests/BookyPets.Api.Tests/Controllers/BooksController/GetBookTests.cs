using System.Net;
using System.Net.Http.Json;
using BookyPets.Api.Tests.Common;
using BookyPets.Api.Tests.Common.Books;
using BookyPets.Contracts.Books;
using BookyPets.Domain.Tests.TestConstants;

namespace BookyPets.Api.Tests.Controllers.BooksController;


[Collection(BookyPetsApiFactoryCollection.CollectionName)]
public class GetBookTests : BookGenres
{
    private readonly HttpClient _client;

    public GetBookTests(BookyPetsApiFactory apiFactory)
    {
        _client = apiFactory.HttpClient;

        apiFactory.ResetDatabase();
    }

    [Fact]
    public async Task GetBook_WhenBookExists_ShouldReturnBook()
    {
        var createResponse = await _client.PostAsJsonAsync("Books", new CreateBookRequest(Constants.Book.Title, Constants.Book.Author, Constants.Book.ContractsGenre, Constants.Book.PageCount));
        var created = await createResponse.Content.ReadFromJsonAsync<BookResponse>();

        var response = await _client.GetAsync($"Books/{created!.Id}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var book = await response.Content.ReadFromJsonAsync<BookResponse>();
        Assert.NotNull(book);
        Assert.Equal(Constants.Book.Title, book.Title);
    }

    [Fact]
    public async Task GetBook_WhenBookDoesNotExist_ShouldReturnNotFound()
    {
        var response = await _client.GetAsync($"Books/{Guid.NewGuid()}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
