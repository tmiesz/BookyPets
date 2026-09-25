using BookyPets.Domain.SessionAggregate;
using BookyPets.Domain.SessionAggregate.Events;
using BookyPets.Domain.Tests.TestConstants;
using BookyPets.Domain.Tests.TestUtils;
using Common.Tests.Sessions;

namespace BookyPets.Domain.Tests;

public class ReadingSessionTests
{
    [Fact]
    public void Complete_WhenSessionIsActive_SucceedsAndRaisesEvent()
    {
        var session = SessionFactory.CreateSession();
        var dateTimeProvider = new FakeDateTimeProvider();

        var result = session.Complete(50, dateTimeProvider.UtcNow);

        Assert.True(result.IsSuccess);
        Assert.Equal(SessionStatus.Completed, session.Status);
        Assert.Equal(50, session.PagesRead);
    }

    [Fact]
    public void Complete_WhenAlreadyCompleted_FailsWithSessionNotActive()
    {
        var session = SessionFactory.CreateSession();
        var dateTimeProvider = new FakeDateTimeProvider();

        session.Complete(50, dateTimeProvider.UtcNow);
        var result = session.Complete(20, dateTimeProvider.UtcNow);

        Assert.False(result.IsSuccess);
        Assert.Equal(SessionErrors.SessionNotActive.Code, result.Error.Code);
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
