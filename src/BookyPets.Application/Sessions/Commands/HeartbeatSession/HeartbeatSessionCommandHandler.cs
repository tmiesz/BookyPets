using BookyPets.Application.Common.Interfaces;
using BookyPets.Domain.Common.Interfaces;
using BookyPets.Domain.SessionAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Sessions.Commands.HeartbeatSession;

public class HeartbeatSessionCommandHandler(
    ISessionsRepository sessionsRepository,
    IUnitOfWork unitOfWork,
    IDateTimeProvider dateTimeProvider,
    ICurrentReaderProvider currentReaderProvider) : IHandler<HeartbeatSessionCommand, Result>
{
    private readonly ISessionsRepository _sessionsRepository = sessionsRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    private readonly ICurrentReaderProvider _currentReaderProvider = currentReaderProvider;

    public async Task<Result> HandleAsync(HeartbeatSessionCommand request, CancellationToken cancellationToken = default)
    {
        var currentReader = _currentReaderProvider.GetCurrentReader();
        var session = await _sessionsRepository.GetActiveSessionAsync(currentReader.Id);

        if (session is null)
            return SessionErrors.SessionNotActive;

        var heartbeatResult = session.Heartbeat(_dateTimeProvider.UtcNow);
        if (!heartbeatResult.IsSuccess)
            return heartbeatResult.Error;

        await _sessionsRepository.UpdateSessionAsync(session);
        await _unitOfWork.CommitChangesAsync();

        return Result.Success;
    }
}
