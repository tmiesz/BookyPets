using BookyPets.Application.Common.Interfaces;
using BookyPets.Domain.Common.Interfaces;
using BookyPets.Domain.SessionAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Sessions.Commands.CompleteSession;

public class CompleteSessionCommandHandler(
    ISessionsRepository sessionsRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider,
    ICurrentReaderProvider currentReaderProvider) : IHandler<CompleteSessionCommand, Result<Session>>
{
    private readonly ISessionsRepository _sessionsRepository = sessionsRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    private readonly ICurrentReaderProvider _currentReaderProvider = currentReaderProvider;

    public async Task<Result<Session>> HandleAsync(CompleteSessionCommand request, CancellationToken cancellationToken = default)
    {
        var session = await _sessionsRepository.GetSessionAsync(request.SessionId);

        if(session is null)
            return new Error(ErrorType.NotFound, "SessionNotFound", "Session was not found");

        if (session.ReaderId != _currentReaderProvider.GetCurrentReader().Id)
            return SessionErrors.NotOwned;

        var completeSessionResult = session.Complete(request.PagesRead, _dateTimeProvider.UtcNow);

        if(!completeSessionResult.IsSuccess)
        {
            return completeSessionResult.Error;
        }

        await _sessionsRepository.UpdateSessionAsync(session);
        await _unitOfWork.CommitChangesAsync();

        return session;
    }
}
