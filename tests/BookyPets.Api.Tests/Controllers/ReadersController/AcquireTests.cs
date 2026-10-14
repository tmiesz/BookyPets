using System.Net;
using BookyPets.Api.Tests.Common;
using BookyPets.Contracts.Books;
using BookyPets.Contracts.Pets;
using BookyPets.Contracts.Readers;

namespace BookyPets.Api.Tests.Controllers.ReadersController;

[Collection(BookyPetsApiFactoryCollection.CollectionName)]
public class AcquireTests
{
    private readonly BookyPetsApiFactory _apiFactory;

    public AcquireTests(BookyPetsApiFactory apiFactory)
    {
        _apiFactory = apiFactory;
        apiFactory.ResetDatabase();
    }

    private async Task<HttpClient> RegisterReaderAsync()
    {
        var auth = await _apiFactory.CreateAnonymousClient().RegisterAsync();

        return _apiFactory.CreateReaderClient(auth.Id);
    }

    [Fact]
    public async Task AcquireBook_WhenBookExists_ShouldReturnProgressAndShowTheBookInTheReadersLibrary()
    {
        var reader = await RegisterReaderAsync();
        var book = await _apiFactory.HttpClient.CreateBookAsync();

        var progressId = await reader.AcquireBookAsync(book.Id);

        var booksResponse = await reader.GetAsync("Readers/books");
        Assert.Equal(HttpStatusCode.OK, booksResponse.StatusCode);
        var books = await booksResponse.ReadRequiredAsync<BookResponse[]>();
        Assert.Equal(book.Id, Assert.Single(books).Id);

        var libraryResponse = await reader.GetAsync("Readers/library");
        Assert.Equal(HttpStatusCode.OK, libraryResponse.StatusCode);
        var library = await libraryResponse.ReadRequiredAsync<ReaderLibraryEntryResponse[]>();
        var entry = Assert.Single(library);
        Assert.Equal(book.Id, entry.Book.Id);
        Assert.Equal(progressId, entry.Progress.Id);
        Assert.Equal(0, entry.Progress.CurrentPage);
        Assert.Equal(book.PageCount, entry.Progress.TotalPages);
    }

    [Fact]
    public async Task AcquireBook_WhenBookIsAlreadyOwned_ShouldReturnConflict()
    {
        var reader = await RegisterReaderAsync();
        var book = await _apiFactory.HttpClient.CreateBookAsync();
        await reader.AcquireBookAsync(book.Id);

        var response = await reader.PostAsync($"Readers/books/{book.Id}/acquire", null);

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        Assert.Equal("Reader.BookAlreadyOwned", await response.ReadProblemCodeAsync());
    }

    [Fact]
    public async Task AcquireBook_WhenBookDoesNotExist_ShouldReturnNotFound()
    {
        var reader = await RegisterReaderAsync();

        var response = await reader.PostAsync($"Readers/books/{Guid.NewGuid()}/acquire", null);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("BookNotFound", await response.ReadProblemCodeAsync());
    }

    [Fact]
    public async Task AcquirePet_WhenPetExists_ShouldShowThePetInTheReadersPets()
    {
        var reader = await RegisterReaderAsync();
        var pet = await _apiFactory.HttpClient.CreatePetAsync();

        await reader.AcquirePetAsync(pet.Id);

        var petsResponse = await reader.GetAsync("Readers/pets");
        Assert.Equal(HttpStatusCode.OK, petsResponse.StatusCode);
        var pets = await petsResponse.ReadRequiredAsync<PetResponse[]>();
        Assert.Equal(pet.Id, Assert.Single(pets).Id);
    }

    [Fact]
    public async Task AcquirePet_WhenPetIsAlreadyOwned_ShouldReturnConflict()
    {
        var reader = await RegisterReaderAsync();
        var pet = await _apiFactory.HttpClient.CreatePetAsync();
        await reader.AcquirePetAsync(pet.Id);

        var response = await reader.PostAsync($"Readers/pets/{pet.Id}/acquire", null);

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        Assert.Equal("Reader.PetAlreadyOwned", await response.ReadProblemCodeAsync());
    }

    [Fact]
    public async Task AcquirePet_WhenPetDoesNotExist_ShouldReturnNotFound()
    {
        var reader = await RegisterReaderAsync();

        var response = await reader.PostAsync($"Readers/pets/{Guid.NewGuid()}/acquire", null);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("PetNotFound", await response.ReadProblemCodeAsync());
    }

    [Fact]
    public async Task GetProgress_WhenProgressDoesNotExist_ShouldReturnNotFound()
    {
        var reader = await RegisterReaderAsync();

        var response = await reader.GetAsync($"Readers/progress/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("ProgressNotFound", await response.ReadProblemCodeAsync());
    }
}
