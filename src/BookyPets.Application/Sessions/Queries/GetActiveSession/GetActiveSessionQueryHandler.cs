using BookyPets.Application.Common.Interfaces;
using BookyPets.Application.Sessions.Common;
using BookyPets.Domain.Common.Interfaces;
using BookyPets.Domain.SessionAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Sessions.Queries.GetActiveSession;

public class GetActiveSessionQueryHandler(
    ISessionsRepository sessionsRepository,
    IDateTimeProvider dateTimeProvider,
    ICurrentReaderProvider currentReaderProvider) : IHandler<GetActiveSessionQuery, Result<ActiveSessionResult?>>
{
    private readonly ISessionsRepository _sessionsRepository = sessionsRepository;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    private readonly ICurrentReaderProvider _currentReaderProvider = currentReaderProvider;

    public async Task<Result<ActiveSessionResult?>> HandleAsync(GetActiveSessionQuery request, CancellationToken cancellationToken = default)
    {
        var currentReader = _currentReaderProvider.GetCurrentReader();
        var session = await _sessionsRepository.GetActiveSessionInfoAsync(currentReader.Id);

        if (session is null)
            return (ActiveSessionResult?)null;

        var now = _dateTimeProvider.UtcNow;
        var isStale = now - session.LastHeartbeatAt > Session.StalenessThreshold;

        var startTimeUtc = DateTime.SpecifyKind(session.StartTime, DateTimeKind.Utc);

        return new ActiveSessionResult(session.Id, session.ProgressId, session.PetId, startTimeUtc, isStale);
    }
}


