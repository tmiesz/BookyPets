using System.Net;
using System.Net.Http.Json;
using BookyPets.Api.Tests.Common;
using BookyPets.Contracts.Authentication;
using BookyPets.Domain.Tests.TestConstants;

namespace BookyPets.Api.Tests.Controllers.AuthenticationController;

[Collection(BookyPetsApiFactoryCollection.CollectionName)]
public class LoginTests
{
    private readonly HttpClient _client;

    public LoginTests(BookyPetsApiFactory apiFactory)
    {
        _client = apiFactory.HttpClient;

        apiFactory.ResetDatabase();
    }

    [Fact]
    public async Task Login_WhenValidCredentials_ShouldReturnToken()
    {
        await _client.PostAsJsonAsync("Authentication/register",
            new RegisterRequest(Constants.Reader.FirstName, Constants.Reader.LastName, Constants.Reader.Email, Constants.Reader.Password));

        var response = await _client.PostAsJsonAsync("Authentication/login",
            new LoginRequest(Constants.Reader.Email, Constants.Reader.Password));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var auth = await response.Content.ReadFromJsonAsync<AuthenticationResponse>();
        Assert.NotNull(auth);
        Assert.False(string.IsNullOrEmpty(auth.Token));
    }

    [Fact]
    public async Task Login_WhenWrongPassword_ShouldReturnUnauthorized()
    {
        await _client.PostAsJsonAsync("Authentication/register",
            new RegisterRequest(Constants.Reader.FirstName, Constants.Reader.LastName, Constants.Reader.Email, Constants.Reader.Password));

        var response = await _client.PostAsJsonAsync("Authentication/login",
            new LoginRequest(Constants.Reader.Email, "wrongpassword"));

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Login_WhenUserDoesNotExist_ShouldReturnUnauthorized()
    {
        var response = await _client.PostAsJsonAsync("Authentication/login",
            new LoginRequest("nobody@nowhere.com", Constants.Reader.Password));

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
