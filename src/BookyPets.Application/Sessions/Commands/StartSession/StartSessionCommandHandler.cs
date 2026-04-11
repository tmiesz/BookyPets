using BookyPets.Application.Common.Interfaces;
using BookyPets.Domain.Common.Interfaces;
using BookyPets.Domain.SessionAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Sessions.Commands.StartSession;

public class StartSessionCommandHandler : IHandler<StartSessionCommand, Result<Session>>
{
    private readonly IProgressesRepository _progressesRepository;
    private readonly ISessionsRepository _sessionsRepository;
    private readonly IBooksRepository _booksRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUnitOfWork _unitOfWork;

    private readonly ICurrentReaderProvider _currentReaderProvider;
    public StartSessionCommandHandler(IUnitOfWork unitOfWork, ISessionsRepository sessionsRepository, IProgressesRepository progressesRepository, IBooksRepository booksRepository, IDateTimeProvider dateTimeProvider, ICurrentReaderProvider currentReaderProvider)
    {
        _unitOfWork = unitOfWork;
        _sessionsRepository = sessionsRepository;
        _progressesRepository = progressesRepository;
        _booksRepository = booksRepository;
        _dateTimeProvider = dateTimeProvider;
        _currentReaderProvider = currentReaderProvider;
    }

    public async Task<Result<Session>> HandleAsync(StartSessionCommand request, CancellationToken cancellationToken = default)
    {
        var currentReader = _currentReaderProvider.GetCurrentReader();

        var progress = await _progressesRepository.GetProgressAsync(request.ProgressId);
        if (progress is null)
            return new Error(ErrorType.NotFound, "ProgressNotFound", "Progression on the book was not found.");

        var book = await _booksRepository.GetBookAsync(progress.BookId);
        if (book is null)
            return new Error(ErrorType.NotFound, "BookNotFound", "Book was not found.");

        var session = new Session(currentReader.Id, request.ProgressId, progress.BookId, book.Genre, _dateTimeProvider.UtcNow, request.PetId);

        await _sessionsRepository.AddSessionAsync(session);
        await _unitOfWork.CommitChangesAsync();

        return session;
    }
}
