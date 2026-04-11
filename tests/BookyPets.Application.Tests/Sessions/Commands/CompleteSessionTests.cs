using BookyPets.Application.Tests.Common;
using BookyPets.Domain.SessionAggregate;
using BookyPets.Domain.Tests.TestConstants;
using BookyPets.Shared.Mediator.Abstractions;
using Common.Tests.Books;
using Common.Tests.Pets;
using Common.Tests.Readers;
using Common.Tests.Sessions;

namespace BookyPets.Application.Tests.Sessions.Commands;

[Collection(MediatorFactoryCollection.CollectionName)]
public class CompleteSessionTests(MediatorFactory mediatorFactory)
{
    private readonly IMediator _mediator = mediatorFactory.CreateMediator();

    [Fact]
    public async Task CompleteSession_WhenValidCommand_ShouldUpdateProgress()
    {
        var registerCommand = ReaderCommandFactory.CreateRegisterCommand();
        var registerResult = await _mediator.SendAsync(registerCommand);
        Assert.True(registerResult.IsSuccess);

        mediatorFactory.FakeCurrentReaderProvider.SetCurrentReader(registerResult.Value.Reader.Id);

        var createBookCommand = BookCommandFactory.CreateCreateBookCommand();
        var createBookResult = await _mediator.SendAsync(createBookCommand);
        Assert.True(createBookResult.IsSuccess);

        var acquireBookCommand = ReaderCommandFactory.CreateAcquireBookCommand(bookId: createBookResult.Value.Id);
        var acquireBookResult = await _mediator.SendAsync(acquireBookCommand);
        Assert.True(acquireBookResult.IsSuccess);

        var startSessionCommand = SessionCommandFactory.CreateStartSessionCommand(progressId: acquireBookResult.Value);
        var startSessionResult = await _mediator.SendAsync(startSessionCommand);

        Assert.True(startSessionResult.IsSuccess);
        Assert.Equal(SessionStatus.Active, startSessionResult.Value.Status);

        var completeSessionCommand = SessionCommandFactory.CreateCompleteSessionCommand(sessionId: startSessionResult.Value.Id, pagesRead: Constants.Session.PagesRead);
        var completeSessionResult = await _mediator.SendAsync(completeSessionCommand);

        Assert.Equal(SessionStatus.Completed, completeSessionResult.Value.Status);
        Assert.Equal(Constants.Session.PagesRead, completeSessionResult.Value.PagesRead);
        Assert.NotNull(completeSessionResult.Value.EndTime);

        Assert.True(startSessionResult.IsSuccess);
        Assert.Equal(Constants.Session.PagesRead, completeSessionResult.Value.PagesRead);

        var getProgressQuery = ReaderCommandFactory.CreateGetProgressQuery(acquireBookResult.Value);
        var progressResult = await _mediator.SendAsync(getProgressQuery);

        Assert.True(progressResult.IsSuccess);
        Assert.Equal(progressResult.Value.CurrentPage, Constants.Session.PagesRead);
    }

    [Fact]
    public async Task CompleteSession_WithPet_ShouldGiveExperience()
    {
        var registerCommand = ReaderCommandFactory.CreateRegisterCommand();
        var registerResult = await _mediator.SendAsync(registerCommand);
        Assert.True(registerResult.IsSuccess);

        mediatorFactory.FakeCurrentReaderProvider.SetCurrentReader(registerResult.Value.Reader.Id);

        var createBookCommand = BookCommandFactory.CreateCreateBookCommand();
        var createBookResult = await _mediator.SendAsync(createBookCommand);
        Assert.True(createBookResult.IsSuccess);

        var acquireBookCommand = ReaderCommandFactory.CreateAcquireBookCommand(bookId: createBookResult.Value.Id);
        var acquireBookResult = await _mediator.SendAsync(acquireBookCommand);
        Assert.True(acquireBookResult.IsSuccess);

        var createPetCommand = PetCommandFactory.CreateCreatePetCommand();
        var createPetResult = await _mediator.SendAsync(createPetCommand);
        Assert.True(createPetResult.IsSuccess);

        var acquirePetCommand = ReaderCommandFactory.CreateAcquirePetCommand(petId: createPetResult.Value.Id);
        var acquirePetResult = await _mediator.SendAsync(acquirePetCommand);
        Assert.True(acquirePetResult.IsSuccess);

        var startSessionCommand = SessionCommandFactory.CreateStartSessionCommand(progressId: acquireBookResult.Value, petId: createPetResult.Value.Id);
        var startSessionResult = await _mediator.SendAsync(startSessionCommand);

        Assert.True(startSessionResult.IsSuccess);
        Assert.Equal(SessionStatus.Active, startSessionResult.Value.Status);

        var completeSessionCommand = SessionCommandFactory.CreateCompleteSessionCommand(sessionId: startSessionResult.Value.Id, pagesRead: Constants.Session.PagesRead);
        var completeSessionResult = await _mediator.SendAsync(completeSessionCommand);

        Assert.Equal(SessionStatus.Completed, completeSessionResult.Value.Status);
        Assert.Equal(Constants.Session.PagesRead, completeSessionResult.Value.PagesRead);
        Assert.NotNull(completeSessionResult.Value.EndTime);

        Assert.True(completeSessionResult.IsSuccess);
        Assert.Equal(Constants.Session.PagesRead, completeSessionResult.Value.PagesRead);

        var getPetQuery = PetCommandFactory.CreateGetPetQuery(petId: createPetResult.Value.Id);
        var petResult = await _mediator.SendAsync(getPetQuery);

        Assert.True(petResult.IsSuccess);
        Assert.True(petResult.Value.Level > 1);
    }
}
