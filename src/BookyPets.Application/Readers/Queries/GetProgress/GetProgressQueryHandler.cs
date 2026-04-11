using BookyPets.Application.Common.Interfaces;
using BookyPets.Domain.BookAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Readers.Queries.GetProgress;

public class GetProgressQueryHandler : IHandler<GetProgressQuery, Result<Progress>>
{
    private readonly IProgressesRepository _progressesRepository;

    public GetProgressQueryHandler(IProgressesRepository progressesRepository)
    {
        _progressesRepository = progressesRepository;
    }

    public async Task<Result<Progress>> HandleAsync(GetProgressQuery query, CancellationToken cancellationToken = default)
    {
        var progress = await _progressesRepository.GetProgressAsync(query.ProgressId);

        if(progress is null)
            return new Error(ErrorType.NotFound, "ProgressNotFound");

        return progress;
    }
}
