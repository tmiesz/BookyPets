using BookyPets.Application.Authentication.Common;
using BookyPets.Application.Tests.Common;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;
using Common.Tests.Readers;
using Common.Tests.TestConstants;

namespace BookyPets.Application.Tests.Authentication.Commands;

[Collection(MediatorFactoryCollection.CollectionName)]
public class LoginTests(MediatorFactory mediatorFactory)
{
    private readonly IMediator _mediator = mediatorFactory.CreateMediator();

    private async Task RegisterDefaultReaderAsync()
    {
        var registerResult = await _mediator.SendAsync(ReaderCommandFactory.CreateRegisterCommand());
        Assert.True(registerResult.IsSuccess);
    }

    [Fact]
    public async Task Login_WhenValidCredentials_ShouldLoginReaderAndIssueToken()
    {
        await RegisterDefaultReaderAsync();

        var result = await _mediator.SendAsync(ReaderCommandFactory.CreateLoginQuery());

        Assert.True(result.IsSuccess);
        Assert.False(string.IsNullOrWhiteSpace(result.Value.Token));
        Assert.Equal(Constants.Reader.Email, result.Value.Reader.Email);
    }

    [Fact]
    public async Task Login_WhenEmailHasDifferentCase_ShouldLoginReader()
    {
        await RegisterDefaultReaderAsync();

        var result = await _mediator.SendAsync(ReaderCommandFactory.CreateLoginQuery(email: "BOB@Smith.com"));

        Assert.True(result.IsSuccess);
        Assert.Equal(Constants.Reader.Email, result.Value.Reader.Email);
    }

    [Fact]
    public async Task Login_WhenEmailIsUnknown_ShouldReturnInvalidCredentials()
    {
        await RegisterDefaultReaderAsync();

        var result = await _mediator.SendAsync(ReaderCommandFactory.CreateLoginQuery(email: "nobody@smith.com"));

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Unauthorized, result.Error.Type);
        Assert.Equal(AuthenticationErrors.InvalidCredentials.Code, result.Error.Code);
    }

    [Fact]
    public async Task Login_WhenPasswordIsWrong_ShouldReturnInvalidCredentials()
    {
        await RegisterDefaultReaderAsync();

        var result = await _mediator.SendAsync(ReaderCommandFactory.CreateLoginQuery(password: "wrongpassword"));

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Unauthorized, result.Error.Type);
        Assert.Equal(AuthenticationErrors.InvalidCredentials.Code, result.Error.Code);
    }
}
