using BookyPets.Application.Common.Interfaces;
using BookyPets.Domain.BookAggregate;
using BookyPets.Domain.Common.Interfaces;
using BookyPets.Domain.ReaderAggregate;
using BookyPets.Domain.SessionAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Sessions.Commands.StartSession;

public class StartSessionCommandHandler(
    IUnitOfWork unitOfWork,
    ISessionsRepository sessionsRepository,
    IProgressesRepository progressesRepository,
    IBooksRepository booksRepository,
    IReadersRepository readersRepository,
    IDateTimeProvider dateTimeProvider,
    ICurrentReaderProvider currentReaderProvider) : IHandler<StartSessionCommand, Result<Session>>
{
    private readonly IProgressesRepository _progressesRepository = progressesRepository;
    private readonly ISessionsRepository _sessionsRepository = sessionsRepository;
    private readonly IBooksRepository _booksRepository = booksRepository;
    private readonly IReadersRepository _readersRepository = readersRepository;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    private readonly ICurrentReaderProvider _currentReaderProvider = currentReaderProvider;

    public async Task<Result<Session>> HandleAsync(StartSessionCommand request, CancellationToken cancellationToken = default)
    {
        var currentReader = _currentReaderProvider.GetCurrentReader();

        var activeSession = await _sessionsRepository.GetActiveSessionAsync(currentReader.Id);

        if (activeSession is not null)
            return SessionErrors.SessionAlreadyActive;

        var progress = await _progressesRepository.GetProgressAsync(request.ProgressId);
        if (progress is null)
            return new Error(ErrorType.NotFound, "ProgressNotFound", "Progression on the book was not found.");

        if (progress.ReaderId != currentReader.Id)
            return ProgressErrors.NotOwned;

        if (progress.Status == BookStatus.Completed)
            return ProgressErrors.AlreadyCompleted;

        if (request.PetId is not null)
        {
            var reader = await _readersRepository.GetByIdAsync(currentReader.Id);
            if (reader is null)
                return new Error(ErrorType.NotFound, "ReaderNotFound", "Reader was not found");

            if (!reader.HasPet(request.PetId.Value))
                return ReaderErrors.PetNotOwned;
        }

        var book = await _booksRepository.GetBookAsync(progress.BookId);
        if (book is null)
            return new Error(ErrorType.NotFound, "BookNotFound", "Book was not found.");

        var session = new Session(currentReader.Id, request.ProgressId, progress.BookId, book.Genre, _dateTimeProvider.UtcNow, request.PetId);

        await _sessionsRepository.AddSessionAsync(session);
        await _unitOfWork.CommitChangesAsync();

        return session;
    }
}
