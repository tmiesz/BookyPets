using System.Net;
using System.Net.Http.Json;
using BookyPets.Api.Tests.Common;
using BookyPets.Api.Tests.Common.Books;
using BookyPets.Contracts.Books;
using BookyPets.Domain.Tests.TestConstants;

namespace BookyPets.Api.Tests.Controllers.BooksController;


[Collection(BookyPetsApiFactoryCollection.CollectionName)]
public class CreateBookTests : BookGenres
{
    private readonly HttpClient _client;

    public CreateBookTests(BookyPetsApiFactory apiFactory)
    {
        _client = apiFactory.HttpClient;

        apiFactory.ResetDatabase();
    }

    [Theory]
    [MemberData(nameof(ListGenres))]
    public async Task CreateBook_WhenValidBook_ShouldCreateBook(Genre genre)
    {
        var createBookRequest = new CreateBookRequest(
            Constants.Book.Title,
            Constants.Book.Author,
            genre,
            Constants.Book.PageCount);

        var response = await _client.PostAsJsonAsync("Books", createBookRequest);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var bookResponse = await response.Content.ReadFromJsonAsync<BookResponse>();
        Assert.NotNull(bookResponse);
        Assert.Equal(Constants.Book.Title, bookResponse.Title);
        Assert.Equal(Constants.Book.Author, bookResponse.Author);
        Assert.Equal(genre, bookResponse.Genre);
        Assert.Equal(Constants.Book.PageCount, bookResponse.PageCount);
    }
}
