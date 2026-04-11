using System.Net;
using System.Net.Http.Json;
using BookyPets.Api.Tests.Common;
using BookyPets.Contracts.Authentication;
using BookyPets.Domain.Tests.TestConstants;

namespace BookyPets.Api.Tests.Controllers.AuthenticationController;

[Collection(BookyPetsApiFactoryCollection.CollectionName)]
public class RegisterTests
{
    private readonly HttpClient _client;

    public RegisterTests(BookyPetsApiFactory apiFactory)
    {
        _client = apiFactory.HttpClient;

        apiFactory.ResetDatabase();
    }

    [Fact]
    public async Task Register_WhenValidRequest_ShouldReturnToken()
    {
        var response = await _client.PostAsJsonAsync("Authentication/register",
            new RegisterRequest(Constants.Reader.FirstName, Constants.Reader.LastName, Constants.Reader.Email, Constants.Reader.Password));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var auth = await response.Content.ReadFromJsonAsync<AuthenticationResponse>();
        Assert.NotNull(auth);
        Assert.Equal(Constants.Reader.Email, auth.Email);
        Assert.False(string.IsNullOrEmpty(auth.Token));
    }

    [Fact]
    public async Task Register_WhenEmailAlreadyExists_ShouldReturnConflict()
    {
        await _client.PostAsJsonAsync("Authentication/register",
            new RegisterRequest(Constants.Reader.FirstName, Constants.Reader.LastName, Constants.Reader.Email, Constants.Reader.Password));

        var response = await _client.PostAsJsonAsync("Authentication/register",
            new RegisterRequest(Constants.Reader.FirstName, Constants.Reader.LastName, Constants.Reader.Email, Constants.Reader.Password));

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }
}
