using BookyPets.Application.Tests.Common;
using BookyPets.Domain.SessionAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;
using Common.Tests.Books;
using Common.Tests.Pets;
using Common.Tests.Readers;
using Common.Tests.Sessions;

namespace BookyPets.Application.Tests.Sessions.Commands;

[Collection(MediatorFactoryCollection.CollectionName)]
public class StartSessionTests(MediatorFactory mediatorFactory)
{
    private readonly IMediator _mediator = mediatorFactory.CreateMediator();

    [Fact]
    public async Task StartSession_WhenValidCommand_ShouldStartSession()
    {
        var registerCommand = ReaderCommandFactory.CreateRegisterCommand();
        var registerResult = await _mediator.SendAsync(registerCommand);
        Assert.True(registerResult.IsSuccess);

        mediatorFactory.FakeCurrentReaderProvider.SetCurrentReader(registerResult.Value.Reader.Id);

        var createBookCommand = BookCommandFactory.CreateCreateBookCommand();
        var createBookResult = await _mediator.SendAsync(createBookCommand);
        Assert.True(createBookResult.IsSuccess);

        var createPetCommand = PetCommandFactory.CreateCreatePetCommand();
        var createPetResult = await _mediator.SendAsync(createPetCommand);
        Assert.True(createPetResult.IsSuccess);

        var acquireBookCommand = ReaderCommandFactory.CreateAcquireBookCommand(bookId: createBookResult.Value.Id);
        var acquireBookResult = await _mediator.SendAsync(acquireBookCommand);
        Assert.True(acquireBookResult.IsSuccess);

        var progressId = acquireBookResult.Value;

        var startSessionCommand = SessionCommandFactory.CreateStartSessionCommand(progressId);
        var startSessionResult = await _mediator.SendAsync(startSessionCommand);

        Assert.True(startSessionResult.IsSuccess);
        Assert.Equal(SessionStatus.Active, startSessionResult.Value.Status);
    }

    [Fact]
    public async Task StartSession_WhenProgressNotFound_ShouldReturnNotFound()
    {
        var startSessionCommand = SessionCommandFactory.CreateStartSessionCommand(Guid.NewGuid());
        var startSessionResult = await _mediator.SendAsync(startSessionCommand);

        Assert.False(startSessionResult.IsSuccess);
        Assert.Equal(ErrorType.NotFound, startSessionResult.Error.Type);
    }
}
