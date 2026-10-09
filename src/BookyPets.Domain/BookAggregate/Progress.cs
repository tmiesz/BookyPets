using BookyPets.Domain.BookAggregate.Events;
using BookyPets.Domain.Common;
using BookyPets.Shared.Result;

namespace BookyPets.Domain.BookAggregate;

public class Progress : AggregateRoot
{
    public Guid ReaderId { get; }
    public Guid BookId { get; }

    public int CurrentPage { get; private set; }
    public int TotalPages { get; }

    public BookStatus Status { get; private set; }

    private Progress()
    {
        Status = null!;
    }

    public Progress(Guid readerId, Book book, Guid? id = null) : base(id ?? Guid.NewGuid())
    {
        ReaderId = readerId;
        BookId = book.Id;
        TotalPages = book.PageCount;
        Status = BookStatus.PlanToRead;
    }

    public Result AddPagesRead(int page)
    {
        if (Status == BookStatus.Completed)
            return ProgressErrors.AlreadyCompleted;
        if (page < 0)
            return ProgressErrors.InvalidPage;
        if (page < CurrentPage)
            return ProgressErrors.CannotGoBackwards;

        if (page == CurrentPage)
            return Result.Success;

        var newPage = Math.Min(page, TotalPages);
        var pagesRead = newPage - CurrentPage;
        CurrentPage = newPage;

        if (CurrentPage == TotalPages)
        {
            Status = BookStatus.Completed;
            _domainEvents.Add(new BookReadEvent(ReaderId, BookId, pagesRead));
        }
        else
        {
            Status = BookStatus.Reading;
            _domainEvents.Add(new PageReadEvent(ReaderId, BookId, pagesRead));
        }

        return Result.Success;
    }

    public Result ChangeStatus(BookStatus newStatus)
    {
        if (Status == BookStatus.Completed)
            return ProgressErrors.AlreadyCompleted;

        if (newStatus == BookStatus.Completed)
            return ProgressErrors.CannotManuallyComplete;

        Status = newStatus;

        return Result.Success;
    }
}
