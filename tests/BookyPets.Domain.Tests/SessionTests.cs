using BookyPets.Domain.SessionAggregate;
using BookyPets.Domain.SessionAggregate.Events;
using Common.Tests.Sessions;
using Common.Tests.TestConstants;
using Common.Tests.TestUtils;

namespace BookyPets.Domain.Tests;

public class SessionTests
{
    [Fact]
    public void Complete_WhenSessionIsActive_SetsStatusPagesReadAndEndTime()
    {
        var session = SessionFactory.CreateSession();
        var dateTimeProvider = new FakeDateTimeProvider();

        var result = session.Complete(50, dateTimeProvider.UtcNow);

        Assert.True(result.IsSuccess);
        Assert.Equal(SessionStatus.Completed, session.Status);
        Assert.Equal(50, session.PagesRead);
        Assert.Equal(dateTimeProvider.UtcNow, session.EndTime);
    }

    [Fact]
    public void Complete_WhenAlreadyCompleted_FailsWithSessionNotActiveAndRaisesNoSecondEvent()
    {
        var session = SessionFactory.CreateSession();
        var dateTimeProvider = new FakeDateTimeProvider();

        session.Complete(50, dateTimeProvider.UtcNow);
        var result = session.Complete(20, dateTimeProvider.UtcNow);

        Assert.False(result.IsSuccess);
        Assert.Equal(SessionErrors.SessionNotActive.Code, result.Error.Code);
        Assert.Equal(50, session.PagesRead);
        Assert.Single(session.PopDomainEvents());
    }

    [Fact]
    public void Complete_WhenSessionWasAbandoned_FailsWithSessionNotActive()
    {
        var session = SessionFactory.CreateSession();
        var dateTimeProvider = new FakeDateTimeProvider();
        session.Abandon(dateTimeProvider.UtcNow);

        var result = session.Complete(50, dateTimeProvider.UtcNow);

        Assert.False(result.IsSuccess);
        Assert.Equal(SessionErrors.SessionNotActive.Code, result.Error.Code);
        Assert.Equal(SessionStatus.Dropped, session.Status);
    }

    [Fact]
    public void Complete_WhenPagesReadIsNegative_FailsAndKeepsSessionActive()
    {
        var session = SessionFactory.CreateSession();
        var dateTimeProvider = new FakeDateTimeProvider();

        var result = session.Complete(-1, dateTimeProvider.UtcNow);

        Assert.False(result.IsSuccess);
        Assert.Equal(SessionErrors.InvalidPagesRead.Code, result.Error.Code);
        Assert.Equal(SessionStatus.Active, session.Status);
        Assert.Null(session.EndTime);
        Assert.Empty(session.PopDomainEvents());
    }

    [Fact]
    public void Complete_WhenPagesReadIsZero_SucceedsAndRaisesEventWithZeroPages()
    {
        var session = SessionFactory.CreateSession();
        var dateTimeProvider = new FakeDateTimeProvider();

        var result = session.Complete(0, dateTimeProvider.UtcNow);

        Assert.True(result.IsSuccess);
        Assert.Equal(SessionStatus.Completed, session.Status);

        var evt = Assert.IsType<SessionCompletedEvent>(Assert.Single(session.PopDomainEvents()));
        Assert.Equal(0, evt.PagesRead);
    }

    [Fact]
    public void Complete_WithCorrectData_RaisesSessionCompletedEvent()
    {
        var dateTimeProvider = new FakeDateTimeProvider();
        var session = SessionFactory.CreateSession(startTime: dateTimeProvider.UtcNow);

        dateTimeProvider.Advance(timeSpan: TimeSpan.FromMinutes(30));
        session.Complete(50, dateTimeProvider.UtcNow);

        var events = session.PopDomainEvents();

        Assert.Single(events);
        var evt = Assert.IsType<SessionCompletedEvent>(events[0]);
        Assert.Equal(30, evt.MinutesRead);
        Assert.Equal(50, evt.PagesRead);
        Assert.Equal(Constants.Pet.Id, evt.PetId);
    }

    [Fact]
    public void Complete_WhenPartOfAMinuteHasPassed_TruncatesMinutesRead()
    {
        var dateTimeProvider = new FakeDateTimeProvider();
        var session = SessionFactory.CreateSession(startTime: dateTimeProvider.UtcNow);

        dateTimeProvider.Advance(TimeSpan.FromMinutes(29) + TimeSpan.FromSeconds(59));
        session.Complete(10, dateTimeProvider.UtcNow);

        var evt = Assert.IsType<SessionCompletedEvent>(Assert.Single(session.PopDomainEvents()));
        Assert.Equal(29, evt.MinutesRead);
    }

    [Fact]
    public void Abandon_WhenSessionIsActive_SetsStatusDroppedAndEndTimeWithoutRaisingEvents()
    {
        var session = SessionFactory.CreateSession();
        var dateTimeProvider = new FakeDateTimeProvider();

        var result = session.Abandon(dateTimeProvider.UtcNow);

        Assert.True(result.IsSuccess);
        Assert.Equal(SessionStatus.Dropped, session.Status);
        Assert.Equal(dateTimeProvider.UtcNow, session.EndTime);
        Assert.Equal(0, session.PagesRead);
        Assert.Empty(session.PopDomainEvents());
    }

    [Fact]
    public void Abandon_WhenAlreadyCompleted_FailsWithSessionNotActive()
    {
        var session = SessionFactory.CreateSession();
        var dateTimeProvider = new FakeDateTimeProvider();
        session.Complete(50, dateTimeProvider.UtcNow);

        var result = session.Abandon(dateTimeProvider.UtcNow);

        Assert.False(result.IsSuccess);
        Assert.Equal(SessionErrors.SessionNotActive.Code, result.Error.Code);
        Assert.Equal(SessionStatus.Completed, session.Status);
    }

    [Fact]
    public void Abandon_WhenAlreadyAbandoned_FailsWithSessionNotActive()
    {
        var session = SessionFactory.CreateSession();
        var dateTimeProvider = new FakeDateTimeProvider();
        session.Abandon(dateTimeProvider.UtcNow);

        var result = session.Abandon(dateTimeProvider.UtcNow);

        Assert.False(result.IsSuccess);
        Assert.Equal(SessionErrors.SessionNotActive.Code, result.Error.Code);
    }

    [Fact]
    public void Heartbeat_WhenSessionIsActive_Succeeds()
    {
        var session = SessionFactory.CreateSession();
        var dateTimeProvider = new FakeDateTimeProvider();

        dateTimeProvider.Advance(TimeSpan.FromMinutes(1));
        var result = session.Heartbeat(dateTimeProvider.UtcNow);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Heartbeat_WhenSessionIsNotActive_FailsWithSessionNotActive()
    {
        var session = SessionFactory.CreateSession();
        var dateTimeProvider = new FakeDateTimeProvider();

        session.Complete(50, dateTimeProvider.UtcNow);
        var result = session.Heartbeat(dateTimeProvider.UtcNow);

        Assert.False(result.IsSuccess);
        Assert.Equal(SessionErrors.SessionNotActive.Code, result.Error.Code);
    }

    [Fact]
    public void Heartbeat_WhenSessionWasAbandoned_FailsWithSessionNotActive()
    {
        var session = SessionFactory.CreateSession();
        var dateTimeProvider = new FakeDateTimeProvider();
        session.Abandon(dateTimeProvider.UtcNow);

        var result = session.Heartbeat(dateTimeProvider.UtcNow);

        Assert.False(result.IsSuccess);
        Assert.Equal(SessionErrors.SessionNotActive.Code, result.Error.Code);
    }

    [Fact]
    public void IsStale_WhenHeartbeatIsRecent_ReturnsFalse()
    {
        var dateTimeProvider = new FakeDateTimeProvider();
        var session = SessionFactory.CreateSession(startTime: dateTimeProvider.UtcNow);

        dateTimeProvider.Advance(TimeSpan.FromMinutes(1));

        Assert.False(session.IsStale(dateTimeProvider.UtcNow));
    }

    [Fact]
    public void IsStale_WhenNoHeartbeatPastThreshold_ReturnsTrue()
    {
        var dateTimeProvider = new FakeDateTimeProvider();
        var session = SessionFactory.CreateSession(startTime: dateTimeProvider.UtcNow);

        dateTimeProvider.Advance(Session.StalenessThreshold + TimeSpan.FromSeconds(1));

        Assert.True(session.IsStale(dateTimeProvider.UtcNow));
    }

    [Fact]
    public void IsStale_WhenExactlyAtThreshold_ReturnsFalse()
    {
        var dateTimeProvider = new FakeDateTimeProvider();
        var session = SessionFactory.CreateSession(startTime: dateTimeProvider.UtcNow);

        dateTimeProvider.Advance(Session.StalenessThreshold);

        Assert.False(session.IsStale(dateTimeProvider.UtcNow));
    }

    [Fact]
    public void IsStale_WhenOneTickPastThreshold_ReturnsTrue()
    {
        var dateTimeProvider = new FakeDateTimeProvider();
        var session = SessionFactory.CreateSession(startTime: dateTimeProvider.UtcNow);

        dateTimeProvider.Advance(Session.StalenessThreshold + TimeSpan.FromTicks(1));

        Assert.True(session.IsStale(dateTimeProvider.UtcNow));
    }

    [Fact]
    public void IsStale_WhenHeartbeatRefreshedPastOriginalThreshold_ReturnsFalse()
    {
        var dateTimeProvider = new FakeDateTimeProvider();
        var session = SessionFactory.CreateSession(startTime: dateTimeProvider.UtcNow);

        dateTimeProvider.Advance(TimeSpan.FromMinutes(4));
        session.Heartbeat(dateTimeProvider.UtcNow);

        dateTimeProvider.Advance(TimeSpan.FromMinutes(4));

        Assert.False(session.IsStale(dateTimeProvider.UtcNow));
    }
}
