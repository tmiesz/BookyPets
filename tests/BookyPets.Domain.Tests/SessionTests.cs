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
}
