using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using BookyPets.Contracts.Authentication;
using BookyPets.Contracts.Books;
using BookyPets.Contracts.Pets;
using BookyPets.Contracts.Readers;
using BookyPets.Contracts.Sessions;
using Common.Tests.TestConstants;

namespace BookyPets.Api.Tests.Common;

public static class ApiTestExtensions
{
    public static async Task<T> ReadRequiredAsync<T>(this HttpResponseMessage response)
    {
        var value = await response.Content.ReadFromJsonAsync<T>();
        Assert.NotNull(value);

        return value!;
    }

    public static async Task<string?> ReadProblemCodeAsync(this HttpResponseMessage response)
    {
        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());

        return document.RootElement.TryGetProperty("code", out var code) ? code.GetString() : null;
    }

    public static async Task EventuallyAsync(Func<Task<bool>> condition, TimeSpan? timeout = null)
    {
        var deadline = DateTime.UtcNow + (timeout ?? TimeSpan.FromSeconds(5));

        while (!await condition())
        {
            if (DateTime.UtcNow > deadline)
                Assert.Fail("The expected state was not reached in time.");

            await Task.Delay(25);
        }
    }

    public static async Task<AuthenticationResponse> RegisterAsync(this HttpClient client, string? email = null)
    {
        var response = await client.PostAsJsonAsync("Authentication/register",
            new RegisterRequest(
                Constants.Reader.FirstName,
                Constants.Reader.LastName,
                email ?? Constants.Reader.Email,
                Constants.Reader.Password));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        return await response.ReadRequiredAsync<AuthenticationResponse>();
    }

    public static async Task<BookResponse> CreateBookAsync(this HttpClient admin, string? title = null, int? pageCount = null)
    {
        var response = await admin.PostAsJsonAsync("Books",
            new CreateBookRequest(
                title ?? Constants.Book.Title,
                Constants.Book.Author,
                Constants.Book.ContractsGenre,
                pageCount ?? Constants.Book.PageCount));

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        return await response.ReadRequiredAsync<BookResponse>();
    }

    public static async Task<PetResponse> CreatePetAsync(this HttpClient admin, string? name = null)
    {
        var response = await admin.PostAsJsonAsync("Pets",
            new CreatePetRequest(
                name ?? Constants.Pet.Name,
                Constants.Pet.ContractSpecies,
                Constants.Pet.FavouriteContractsGenre));

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        return await response.ReadRequiredAsync<PetResponse>();
    }

    public static async Task<Guid> AcquireBookAsync(this HttpClient reader, Guid bookId)
    {
        var response = await reader.PostAsync($"Readers/books/{bookId}/acquire", null);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var acquired = await response.ReadRequiredAsync<AcquireBookResponse>();

        await EventuallyAsync(async () =>
            (await reader.GetAsync($"Readers/progress/{acquired.ProgressId}")).IsSuccessStatusCode);

        return acquired.ProgressId;
    }

    public static async Task AcquirePetAsync(this HttpClient reader, Guid petId)
    {
        var response = await reader.PostAsync($"Readers/pets/{petId}/acquire", null);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    public static async Task<SessionResponse> StartSessionAsync(this HttpClient reader, Guid progressId, Guid? petId = null)
    {
        var response = await reader.PostAsJsonAsync("Sessions/start", new StartSessionRequest(progressId, petId));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        return await response.ReadRequiredAsync<SessionResponse>();
    }

    public static async Task<SessionResponse> CompleteSessionAsync(this HttpClient reader, Guid sessionId, int pagesRead)
    {
        var response = await reader.PostAsJsonAsync("Sessions/complete", new CompleteSessionRequest(sessionId, pagesRead));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        return await response.ReadRequiredAsync<SessionResponse>();
    }

    public static async Task<SessionResponse> AbandonSessionAsync(this HttpClient reader, Guid sessionId)
    {
        var response = await reader.PostAsJsonAsync("Sessions/abandon", new AbandonSessionRequest(sessionId));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        return await response.ReadRequiredAsync<SessionResponse>();
    }

    public static async Task<ProgressResponse> WaitForProgressAsync(this HttpClient reader, Guid progressId, int expectedCurrentPage)
    {
        ProgressResponse? latest = null;

        await EventuallyAsync(async () =>
        {
            var response = await reader.GetAsync($"Readers/progress/{progressId}");

            if (!response.IsSuccessStatusCode)
                return false;

            latest = await response.Content.ReadFromJsonAsync<ProgressResponse>();

            return latest?.CurrentPage == expectedCurrentPage;
        });

        return latest!;
    }

    public static async Task<PetResponse> WaitForPetLevelAsync(this HttpClient reader, Guid petId, int expectedLevel)
    {
        PetResponse? latest = null;

        await EventuallyAsync(async () =>
        {
            var response = await reader.GetAsync($"Pets/{petId}");

            if (!response.IsSuccessStatusCode)
                return false;

            latest = await response.Content.ReadFromJsonAsync<PetResponse>();

            return latest?.Level == expectedLevel;
        });

        return latest!;
    }
}
