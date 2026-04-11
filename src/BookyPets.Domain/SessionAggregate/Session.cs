using BookyPets.Domain.BookAggregate;
using BookyPets.Domain.Common;
using BookyPets.Domain.Common.Interfaces;
using BookyPets.Domain.SessionAggregate.Events;
using BookyPets.Shared.Result;

namespace BookyPets.Domain.SessionAggregate;

public class Session : AggregateRoot
{
    private readonly Guid _readerId;
    private readonly Guid _progressId;
    private readonly Guid _bookId;
    private readonly Genre _genre;
    private readonly Guid? _petId;
    private readonly DateTime _startTime;

    public SessionStatus Status { get; private set; }
    public int PagesRead { get; private set; }
    public DateTime? EndTime { get; private set; }

    private Session()
    {
        _genre = null!;
        Status = null!;
    }

    public Session(
        Guid readerId,
        Guid progressId,
        Guid bookId,
        Genre genre,
        DateTime startTime,
        Guid? petId = null,
        Guid? id = null) : base(id ?? Guid.NewGuid())
    {
        _readerId = readerId;
        _progressId = progressId;
        _bookId = bookId;
        _genre = genre;
        _petId = petId;
        _startTime = startTime;
        Status = SessionStatus.Active;
    }

    public Result Complete(int pagesRead, DateTime now)
    {
        if (Status != SessionStatus.Active)
            return SessionErrors.SessionNotActive;
        if (pagesRead < 0)
            return SessionErrors.InvalidPagesRead;

        PagesRead = pagesRead;
        EndTime = now;
        Status = SessionStatus.Completed;

        var minutesRead = (int)(EndTime.Value - _startTime).TotalMinutes;

        _domainEvents.Add(new SessionCompletedEvent(
            _readerId,
            _petId,
            _progressId,
            _bookId,
            _genre,
            pagesRead,
            minutesRead
        ));

        return Result.Success;
    }

    public Result Abandon(DateTime now)
    {
        if (Status != SessionStatus.Active)
            return SessionErrors.SessionNotActive;

        Status = SessionStatus.Dropped;
        EndTime = now;

        return Result.Success;
    }
}
