using System.Net;
using System.Net.Http.Json;
using BookyPets.Api.Tests.Common;
using BookyPets.Contracts.Authentication;
using BookyPets.Contracts.Readers;
using BookyPets.Domain.Tests.TestConstants;

namespace BookyPets.Api.Tests.Controllers.ReadersController;

[Collection(BookyPetsApiFactoryCollection.CollectionName)]
public class ReadersTests
{
    private readonly HttpClient _client;

    public ReadersTests(BookyPetsApiFactory apiFactory)
    {
        _client = apiFactory.HttpClient;
        apiFactory.ResetDatabase();
    }

    [Fact]
    public async Task ChangeAccountType_WhenValidRequest_ShouldReturnUpdatedReader()
    {
        var registerResponse = await _client.PostAsJsonAsync("Authentication/register",
            new RegisterRequest(Constants.Reader.FirstName, Constants.Reader.LastName, Constants.Reader.Email, Constants.Reader.Password));

        var auth = await registerResponse.Content.ReadFromJsonAsync<AuthenticationResponse>();

        var response = await _client.PatchAsJsonAsync($"Readers/{auth!.Id}/account",
            new ChangeAccountTypeRequest(AccountType.Pro));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var reader = await response.Content.ReadFromJsonAsync<ReaderResponse>();
        Assert.NotNull(reader);
        Assert.Equal(AccountType.Pro, reader.AccountType);
    }

    [Fact]
    public async Task ChangeAccountType_WhenReaderDoesNotExist_ShouldReturnNotFound()
    {
        var response = await _client.PatchAsJsonAsync($"Readers/{Guid.NewGuid()}/account",
            new ChangeAccountTypeRequest(AccountType.Pro));

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
