using BookyPets.Application.Common.Interfaces;
using BookyPets.Domain.Common.Interfaces;
using BookyPets.Domain.SessionAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Sessions.Commands.AbandonSession;

public class AbandonSessionCommandHandler : IHandler<AbandonSessionCommand, Result<Session>>
{
    private readonly ISessionsRepository _sessionsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public AbandonSessionCommandHandler(ISessionsRepository sessionsRepository, IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider)
    {
        _sessionsRepository = sessionsRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<Session>> HandleAsync(AbandonSessionCommand request, CancellationToken cancellationToken = default)
    {
        var session = await _sessionsRepository.GetSessionAsync(request.SessionId);

        if(session is null)
            return new Error(ErrorType.NotFound, "SessionNotFound", "Session was not found");

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
