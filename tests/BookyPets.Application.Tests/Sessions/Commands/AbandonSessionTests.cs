using BookyPets.Application.Tests.Common;
using BookyPets.Domain.SessionAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;
using Common.Tests.Sessions;
using Common.Tests.TestConstants;

namespace BookyPets.Application.Tests.Sessions.Commands;

[Collection(MediatorFactoryCollection.CollectionName)]
public class AbandonSessionTests(MediatorFactory mediatorFactory)
{
    private readonly TestScenario _scenario = mediatorFactory.CreateScenario();

    private IMediator Mediator => _scenario.Mediator;

    [Fact]
    public async Task AbandonSession_WhenSessionIsActive_ShouldDropTheSessionAndLeaveProgressUntouched()
    {
        await _scenario.RegisterReaderAsync();
        var progressId = await _scenario.AcquireBookAsync();
        var session = await _scenario.StartSessionAsync(progressId);

        var result = await Mediator.SendAsync(SessionCommandFactory.CreateAbandonSessionCommand(session.Id));

        Assert.True(result.IsSuccess);
        Assert.Equal(SessionStatus.Dropped, result.Value.Status);
        Assert.NotNull(result.Value.EndTime);
        Assert.Null(await _scenario.GetActiveSessionAsync());
        Assert.Equal(0, (await _scenario.GetProgressAsync(progressId)).CurrentPage);
    }

    [Fact]
    public async Task AbandonSession_WhenSessionDoesNotExist_ShouldReturnNotFound()
    {
        await _scenario.RegisterReaderAsync();

        var result = await Mediator.SendAsync(SessionCommandFactory.CreateAbandonSessionCommand(Guid.NewGuid()));

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.NotFound, result.Error.Type);
        Assert.Equal("SessionNotFound", result.Error.Code);
    }

    [Fact]
    public async Task AbandonSession_WhenSessionBelongsToAnotherReader_ShouldReturnForbiddenAndLeaveTheSessionActive()
    {
        var ownerId = await _scenario.RegisterReaderAsync();
        var progressId = await _scenario.AcquireBookAsync();
        var session = await _scenario.StartSessionAsync(progressId);

        await _scenario.RegisterReaderAsync(Constants.Reader.OtherEmail);
        var result = await Mediator.SendAsync(SessionCommandFactory.CreateAbandonSessionCommand(session.Id));

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Forbidden, result.Error.Type);
        Assert.Equal(SessionErrors.NotOwned.Code, result.Error.Code);

        _scenario.SwitchTo(ownerId);
        Assert.NotNull(await _scenario.GetActiveSessionAsync());
    }

    [Fact]
    public async Task AbandonSession_WhenSessionWasAlreadyAbandoned_ShouldReturnSessionNotActive()
    {
        await _scenario.RegisterReaderAsync();
        var progressId = await _scenario.AcquireBookAsync();
        var session = await _scenario.StartSessionAsync(progressId);
        await Mediator.SendAsync(SessionCommandFactory.CreateAbandonSessionCommand(session.Id));

        var result = await Mediator.SendAsync(SessionCommandFactory.CreateAbandonSessionCommand(session.Id));

        Assert.False(result.IsSuccess);
        Assert.Equal(SessionErrors.SessionNotActive.Code, result.Error.Code);
    }

    [Fact]
    public async Task AbandonSession_WhenSessionWasAlreadyCompleted_ShouldReturnSessionNotActive()
    {
        await _scenario.RegisterReaderAsync();
        var progressId = await _scenario.AcquireBookAsync();
        var session = await _scenario.StartSessionAsync(progressId);
        await Mediator.SendAsync(SessionCommandFactory.CreateCompleteSessionCommand(session.Id, pagesRead: 10));

        var result = await Mediator.SendAsync(SessionCommandFactory.CreateAbandonSessionCommand(session.Id));

        Assert.False(result.IsSuccess);
        Assert.Equal(SessionErrors.SessionNotActive.Code, result.Error.Code);
        Assert.Equal(10, (await _scenario.GetProgressAsync(progressId)).CurrentPage);
    }

    [Fact]
    public async Task AbandonSession_ShouldAllowStartingANewSession()
    {
        await _scenario.RegisterReaderAsync();
        var progressId = await _scenario.AcquireBookAsync();
        var session = await _scenario.StartSessionAsync(progressId);
        await Mediator.SendAsync(SessionCommandFactory.CreateAbandonSessionCommand(session.Id));

        var result = await Mediator.SendAsync(SessionCommandFactory.CreateStartSessionCommand(progressId));

        Assert.True(result.IsSuccess);
        Assert.NotEqual(session.Id, result.Value.Id);
    }
}
