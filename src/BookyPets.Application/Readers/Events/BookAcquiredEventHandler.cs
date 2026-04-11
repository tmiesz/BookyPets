using BookyPets.Application.Common.Interfaces;
using BookyPets.Domain.BookAggregate;
using BookyPets.Domain.BookAggregate.Events;
using BookyPets.Shared.Mediator.Abstractions;

namespace BookyPets.Application.Readers.Events;

public class BookAcquiredEventHandler : INotificationHandler<BookAcquiredEvent>
{
    private readonly IBooksRepository _booksRepository;
    private readonly IProgressesRepository _progressesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BookAcquiredEventHandler(IProgressesRepository progressesRepository, IBooksRepository booksRepository, IUnitOfWork unitOfWork)
    {
        _progressesRepository = progressesRepository;
        _booksRepository = booksRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(BookAcquiredEvent notification, CancellationToken cancellationToken = default)
    {
        var book = await _booksRepository.GetBookAsync(notification.BookId) ?? throw new InvalidOperationException();
        var progress = new Progress(notification.ReaderId, book, notification.ProgressId);

        await _progressesRepository.AddProgressAsync(progress);
        await _unitOfWork.CommitChangesAsync();
    }
}
