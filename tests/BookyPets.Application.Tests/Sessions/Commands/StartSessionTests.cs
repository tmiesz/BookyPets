using BookyPets.Application.Tests.Common;
using BookyPets.Domain.BookAggregate;
using BookyPets.Domain.ReaderAggregate;
using BookyPets.Domain.SessionAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;
using Common.Tests.Sessions;
using Common.Tests.TestConstants;

namespace BookyPets.Application.Tests.Sessions.Commands;

[Collection(MediatorFactoryCollection.CollectionName)]
public class StartSessionTests(MediatorFactory mediatorFactory)
{
    private readonly TestScenario _scenario = mediatorFactory.CreateScenario();

    private IMediator Mediator => _scenario.Mediator;

    [Fact]
    public async Task StartSession_WhenValidCommand_ShouldStartSession()
    {
        await _scenario.RegisterReaderAsync();
        var progressId = await _scenario.AcquireBookAsync();

        var startSessionResult = await Mediator.SendAsync(SessionCommandFactory.CreateStartSessionCommand(progressId));

        Assert.True(startSessionResult.IsSuccess);
        Assert.Equal(SessionStatus.Active, startSessionResult.Value.Status);

        var activeSession = await _scenario.GetActiveSessionAsync();
        Assert.NotNull(activeSession);
        Assert.Equal(startSessionResult.Value.Id, activeSession.Id);
        Assert.Equal(progressId, activeSession.ProgressId);
    }

    [Fact]
    public async Task StartSession_WithAnOwnedPet_ShouldStartSessionWithThePet()
    {
        await _scenario.RegisterReaderAsync();
        var progressId = await _scenario.AcquireBookAsync();
        var petId = await _scenario.AcquirePetAsync();

        var session = await _scenario.StartSessionAsync(progressId, petId);

        var activeSession = await _scenario.GetActiveSessionAsync();
        Assert.NotNull(activeSession);
        Assert.Equal(session.Id, activeSession.Id);
        Assert.Equal(petId, activeSession.PetId);
    }

    [Fact]
    public async Task StartSession_WhenProgressNotFound_ShouldReturnNotFound()
    {
        await _scenario.RegisterReaderAsync();

        var startSessionResult = await Mediator.SendAsync(SessionCommandFactory.CreateStartSessionCommand(Guid.NewGuid()));

        Assert.False(startSessionResult.IsSuccess);
        Assert.Equal(ErrorType.NotFound, startSessionResult.Error.Type);
        Assert.Equal("ProgressNotFound", startSessionResult.Error.Code);
    }

    [Fact]
    public async Task StartSession_WhenProgressBelongsToAnotherReader_ShouldReturnForbidden()
    {
        await _scenario.RegisterReaderAsync();
        var progressId = await _scenario.AcquireBookAsync();
        await _scenario.RegisterReaderAsync(Constants.Reader.OtherEmail);

        var result = await Mediator.SendAsync(SessionCommandFactory.CreateStartSessionCommand(progressId));

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Forbidden, result.Error.Type);
        Assert.Equal(ProgressErrors.NotOwned.Code, result.Error.Code);
    }

    [Fact]
    public async Task StartSession_WhenPetIsNotOwnedByTheReader_ShouldReturnForbidden()
    {
        await _scenario.RegisterReaderAsync();
        var progressId = await _scenario.AcquireBookAsync();
        var petId = await _scenario.CreatePetAsync();

        var result = await Mediator.SendAsync(SessionCommandFactory.CreateStartSessionCommand(progressId, petId));

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Forbidden, result.Error.Type);
        Assert.Equal(ReaderErrors.PetNotOwned.Code, result.Error.Code);
    }

    [Fact]
    public async Task StartSession_WhenPetDoesNotExist_ShouldReturnForbidden()
    {
        await _scenario.RegisterReaderAsync();
        var progressId = await _scenario.AcquireBookAsync();

        var result = await Mediator.SendAsync(SessionCommandFactory.CreateStartSessionCommand(progressId, Guid.NewGuid()));

        Assert.False(result.IsSuccess);
        Assert.Equal(ReaderErrors.PetNotOwned.Code, result.Error.Code);
    }

    [Fact]
    public async Task StartSession_WhenReaderAlreadyHasAnActiveSession_ShouldReturnConflictEvenForAnotherBook()
    {
        await _scenario.RegisterReaderAsync();
        var firstProgressId = await _scenario.AcquireBookAsync();
        var secondBookId = await _scenario.CreateBookAsync(title: Constants.Book.SecondTitle);
        var secondProgressId = await _scenario.AcquireBookAsync(secondBookId);
        await _scenario.StartSessionAsync(firstProgressId);

        var result = await Mediator.SendAsync(SessionCommandFactory.CreateStartSessionCommand(secondProgressId));

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Conflict, result.Error.Type);
        Assert.Equal(SessionErrors.SessionAlreadyActive.Code, result.Error.Code);
    }

    [Fact]
    public async Task StartSession_WhenTheBookIsAlreadyCompleted_ShouldReturnAlreadyCompleted()
    {
        await _scenario.RegisterReaderAsync();
        var progressId = await _scenario.AcquireBookAsync();
        var session = await _scenario.StartSessionAsync(progressId);

        var completeResult = await Mediator.SendAsync(
            SessionCommandFactory.CreateCompleteSessionCommand(session.Id, pagesRead: Constants.Book.PageCount));
        Assert.True(completeResult.IsSuccess);

        var result = await Mediator.SendAsync(SessionCommandFactory.CreateStartSessionCommand(progressId));

        Assert.False(result.IsSuccess);
        Assert.Equal(ProgressErrors.AlreadyCompleted.Code, result.Error.Code);
    }

    [Fact]
    public async Task StartSession_AfterThePreviousSessionWasCompleted_ShouldStartANewSession()
    {
        await _scenario.RegisterReaderAsync();
        var progressId = await _scenario.AcquireBookAsync();
        var firstSession = await _scenario.StartSessionAsync(progressId);
        await Mediator.SendAsync(SessionCommandFactory.CreateCompleteSessionCommand(firstSession.Id, pagesRead: 10));

        var result = await Mediator.SendAsync(SessionCommandFactory.CreateStartSessionCommand(progressId));

        Assert.True(result.IsSuccess);
        Assert.NotEqual(firstSession.Id, result.Value.Id);
    }
}
