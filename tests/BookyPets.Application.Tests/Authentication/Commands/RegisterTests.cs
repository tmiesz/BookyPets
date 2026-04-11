using BookyPets.Application.Tests.Common;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;
using Common.Tests.Readers;

namespace BookyPets.Application.Tests.Authentication.Commands;

[Collection(MediatorFactoryCollection.CollectionName)]
public class RegisterTests(MediatorFactory mediatorFactory)
{
    private readonly IMediator _mediator = mediatorFactory.CreateMediator();

    [Fact]
    public async Task Register_WhenValidCommand_ShouldRegisterReader()
    {
        var registerComamnd = ReaderCommandFactory.CreateRegisterCommand();

        var result = await _mediator.SendAsync(registerComamnd);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task Register_WhenEmailExists_ShouldReturnConflict()
    {
        var registerComamnd = ReaderCommandFactory.CreateRegisterCommand();

        await _mediator.SendAsync(registerComamnd);
        var result = await _mediator.SendAsync(registerComamnd);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Conflict, result.Error.Type);
    }
}
