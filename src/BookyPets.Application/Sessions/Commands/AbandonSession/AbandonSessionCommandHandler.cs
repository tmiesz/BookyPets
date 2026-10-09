using BookyPets.Application.Common.Interfaces;
using BookyPets.Domain.Common.Interfaces;
using BookyPets.Domain.SessionAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Sessions.Commands.AbandonSession;

public class AbandonSessionCommandHandler(
    ISessionsRepository sessionsRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider,
    ICurrentReaderProvider currentReaderProvider) : IHandler<AbandonSessionCommand, Result<Session>>
{
    private readonly ISessionsRepository _sessionsRepository = sessionsRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    private readonly ICurrentReaderProvider _currentReaderProvider = currentReaderProvider;

    public async Task<Result<Session>> HandleAsync(AbandonSessionCommand request, CancellationToken cancellationToken = default)
    {
        var session = await _sessionsRepository.GetSessionAsync(request.SessionId);

        if(session is null)
            return new Error(ErrorType.NotFound, "SessionNotFound", "Session was not found");

        if (session.ReaderId != _currentReaderProvider.GetCurrentReader().Id)
            return SessionErrors.NotOwned;

        var abandonSessionResult = session.Abandon(_dateTimeProvider.UtcNow);

        if(!abandonSessionResult.IsSuccess)
        {
            return abandonSessionResult.Error;
        }

        await _sessionsRepository.UpdateSessionAsync(session);
        await _unitOfWork.CommitChangesAsync();

        return session;
    }
}
