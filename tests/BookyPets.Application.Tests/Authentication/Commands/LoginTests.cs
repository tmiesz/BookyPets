using BookyPets.Application.Tests.Common;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;
using Common.Tests.Readers;

namespace BookyPets.Application.Tests.Authentication.Commands;

[Collection(MediatorFactoryCollection.CollectionName)]
public class LoginTests(MediatorFactory mediatorFactory)
{
    private readonly IMediator _mediator = mediatorFactory.CreateMediator();

    [Fact]
    public async Task Login_WhenValidCommand_ShouldLoginReader()
    {
        var registerComamnd = ReaderCommandFactory.CreateRegisterCommand();
        await _mediator.SendAsync(registerComamnd);

        var loginQuery = ReaderCommandFactory.CreateLoginQuery();
        var result = await _mediator.SendAsync(loginQuery);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Login_WhenEmailIncorrect_ShouldReturnUnauthorized()
    {
        var loginQuery = ReaderCommandFactory.CreateLoginQuery(email: "badTestEmail");

        var result = await _mediator.SendAsync(loginQuery);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Unauthorized, result.Error.Type);
    }
}
