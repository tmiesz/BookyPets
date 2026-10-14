using BookyPets.Application.Tests.Common;
using BookyPets.Domain.BookAggregate;
using BookyPets.Domain.SessionAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;
using Common.Tests.Sessions;
using Common.Tests.TestConstants;

namespace BookyPets.Application.Tests.Sessions.Commands;

[Collection(MediatorFactoryCollection.CollectionName)]
public class CompleteSessionTests(MediatorFactory mediatorFactory)
{
    private readonly TestScenario _scenario = mediatorFactory.CreateScenario();

    private IMediator Mediator => _scenario.Mediator;

    [Fact]
    public async Task CompleteSession_WhenValidCommand_ShouldUpdateProgress()
    {
        await _scenario.RegisterReaderAsync();
        var progressId = await _scenario.AcquireBookAsync();
        var session = await _scenario.StartSessionAsync(progressId);

        var completeResult = await Mediator.SendAsync(
            SessionCommandFactory.CreateCompleteSessionCommand(session.Id, Constants.Session.PagesRead));

        Assert.True(completeResult.IsSuccess);
        Assert.Equal(SessionStatus.Completed, completeResult.Value.Status);
        Assert.Equal(Constants.Session.PagesRead, completeResult.Value.PagesRead);
        Assert.NotNull(completeResult.Value.EndTime);

        var progress = await _scenario.GetProgressAsync(progressId);
        Assert.Equal(Constants.Session.PagesRead, progress.CurrentPage);
        Assert.Equal(BookStatus.Reading, progress.Status);

        Assert.Null(await _scenario.GetActiveSessionAsync());
    }

    [Fact]
    public async Task CompleteSession_WithPetThatLovesTheGenre_ShouldGiveBonusExperience()
    {
        await _scenario.RegisterReaderAsync();
        var progressId = await _scenario.AcquireBookAsync();
        var petId = await _scenario.AcquirePetAsync();
        var session = await _scenario.StartSessionAsync(progressId, petId);

        await Mediator.SendAsync(SessionCommandFactory.CreateCompleteSessionCommand(session.Id, pagesRead: 50));

        var pet = await _scenario.GetPetAsync(petId);
        Assert.Equal(3, pet.Level);
        Assert.Equal(90, pet.Experience);
    }

    [Fact]
    public async Task CompleteSession_WithPetThatDoesNotLoveTheGenre_ShouldGiveNoBonus()
    {
        await _scenario.RegisterReaderAsync();
        var progressId = await _scenario.AcquireBookAsync();
        var petId = await _scenario.CreatePetAsync(genre: Genre.Fantasy);
        await _scenario.AcquirePetAsync(petId);
        var session = await _scenario.StartSessionAsync(progressId, petId);

        await Mediator.SendAsync(SessionCommandFactory.CreateCompleteSessionCommand(session.Id, pagesRead: 50));

        var pet = await _scenario.GetPetAsync(petId);
        Assert.Equal(2, pet.Level);
        Assert.Equal(160, pet.Experience);
    }

    [Fact]
    public async Task CompleteSession_WithZeroPages_ShouldCompleteTheSessionWithoutChangingProgress()
    {
        await _scenario.RegisterReaderAsync();
        var progressId = await _scenario.AcquireBookAsync();
        var session = await _scenario.StartSessionAsync(progressId);

        var result = await Mediator.SendAsync(SessionCommandFactory.CreateCompleteSessionCommand(session.Id, pagesRead: 0));

        Assert.True(result.IsSuccess);
        Assert.Equal(SessionStatus.Completed, result.Value.Status);
        Assert.Equal(0, result.Value.PagesRead);

        var progress = await _scenario.GetProgressAsync(progressId);
        Assert.Equal(0, progress.CurrentPage);
    }

    [Fact]
    public async Task CompleteSession_WhenPagesReadIsNegative_ShouldReturnValidationErrorAndKeepSessionActive()
    {
        await _scenario.RegisterReaderAsync();
        var progressId = await _scenario.AcquireBookAsync();
        var session = await _scenario.StartSessionAsync(progressId);

        var result = await Mediator.SendAsync(SessionCommandFactory.CreateCompleteSessionCommand(session.Id, pagesRead: -5));

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Validation, result.Error.Type);
        Assert.Equal("PagesRead", result.Error.Code);

        var activeSession = await _scenario.GetActiveSessionAsync();
        Assert.NotNull(activeSession);
        Assert.Equal(session.Id, activeSession.Id);
    }

    [Fact]
    public async Task CompleteSession_WhenPagesReadGoesPastTheEndOfTheBook_ShouldClampProgressAndCompleteTheBook()
    {
        await _scenario.RegisterReaderAsync();
        var progressId = await _scenario.AcquireBookAsync();
        var session = await _scenario.StartSessionAsync(progressId);

        var result = await Mediator.SendAsync(
            SessionCommandFactory.CreateCompleteSessionCommand(session.Id, pagesRead: Constants.Book.PageCount + 400));

        Assert.True(result.IsSuccess);

        var progress = await _scenario.GetProgressAsync(progressId);
        Assert.Equal(Constants.Book.PageCount, progress.CurrentPage);
        Assert.Equal(BookStatus.Completed, progress.Status);
    }

    [Fact]
    public async Task CompleteSession_WhenSessionDoesNotExist_ShouldReturnNotFound()
    {
        await _scenario.RegisterReaderAsync();

        var result = await Mediator.SendAsync(SessionCommandFactory.CreateCompleteSessionCommand(Guid.NewGuid()));

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.NotFound, result.Error.Type);
        Assert.Equal("SessionNotFound", result.Error.Code);
    }

    [Fact]
    public async Task CompleteSession_WhenSessionBelongsToAnotherReader_ShouldReturnForbiddenAndLeaveTheSessionActive()
    {
        var ownerId = await _scenario.RegisterReaderAsync();
        var progressId = await _scenario.AcquireBookAsync();
        var session = await _scenario.StartSessionAsync(progressId);

        await _scenario.RegisterReaderAsync(Constants.Reader.OtherEmail);
        var result = await Mediator.SendAsync(SessionCommandFactory.CreateCompleteSessionCommand(session.Id));

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Forbidden, result.Error.Type);
        Assert.Equal(SessionErrors.NotOwned.Code, result.Error.Code);

        _scenario.SwitchTo(ownerId);
        var activeSession = await _scenario.GetActiveSessionAsync();
        Assert.NotNull(activeSession);
        Assert.Equal(session.Id, activeSession.Id);
        Assert.Equal(0, (await _scenario.GetProgressAsync(progressId)).CurrentPage);
    }

    [Fact]
    public async Task CompleteSession_WhenSessionIsAlreadyCompleted_ShouldReturnSessionNotActive()
    {
        await _scenario.RegisterReaderAsync();
        var progressId = await _scenario.AcquireBookAsync();
        var session = await _scenario.StartSessionAsync(progressId);
        await Mediator.SendAsync(SessionCommandFactory.CreateCompleteSessionCommand(session.Id, pagesRead: 10));

        var result = await Mediator.SendAsync(SessionCommandFactory.CreateCompleteSessionCommand(session.Id, pagesRead: 20));

        Assert.False(result.IsSuccess);
        Assert.Equal(SessionErrors.SessionNotActive.Code, result.Error.Code);
        Assert.Equal(10, (await _scenario.GetProgressAsync(progressId)).CurrentPage);
    }
}
