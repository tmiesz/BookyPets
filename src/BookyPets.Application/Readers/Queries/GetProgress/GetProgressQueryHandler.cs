using BookyPets.Application.Common.Interfaces;
using BookyPets.Domain.BookAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Readers.Queries.GetProgress;

public class GetProgressQueryHandler(
    IProgressesRepository progressesRepository,
    ICurrentReaderProvider currentReaderProvider) : IHandler<GetProgressQuery, Result<Progress>>
{
    private readonly IProgressesRepository _progressesRepository = progressesRepository;
    private readonly ICurrentReaderProvider _currentReaderProvider = currentReaderProvider;

    public async Task<Result<Progress>> HandleAsync(GetProgressQuery query, CancellationToken cancellationToken = default)
    {
        var progress = await _progressesRepository.GetProgressAsync(query.ProgressId);

        if(progress is null)
            return new Error(ErrorType.NotFound, "ProgressNotFound");

        if (progress.ReaderId != _currentReaderProvider.GetCurrentReader().Id)
            return ProgressErrors.NotOwned;

        return progress;
    }
}
