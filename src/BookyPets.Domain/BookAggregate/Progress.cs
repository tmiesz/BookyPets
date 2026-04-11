using BookyPets.Domain.BookAggregate.Events;
using BookyPets.Domain.Common;
using BookyPets.Shared.Result;

namespace BookyPets.Domain.BookAggregate;

public class Progress : AggregateRoot
{
    private readonly Guid _readerId;
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
        _readerId = readerId;
        BookId = book.Id;
        TotalPages = book.PageCount;
        Status = BookStatus.PlanToRead;
    }

    public Result AddPagesRead(int page)
    {
        if (Status == BookStatus.Completed)
            return ProgressErrors.AlreadyCompleted;
        if (page <= 0)
            return ProgressErrors.InvalidPage;

        if (page == CurrentPage)
            return Result.Success;

        var pagesRead = page - CurrentPage;
        CurrentPage = page;

        if (CurrentPage == TotalPages)
        {
            Status = BookStatus.Completed;
            _domainEvents.Add(new BookReadEvent(_readerId, BookId, pagesRead));
        }
        else
        {
            Status = BookStatus.Reading;
            _domainEvents.Add(new PageReadEvent(_readerId, BookId, pagesRead));
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
