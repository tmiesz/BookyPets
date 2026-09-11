using BookyPets.Application.Common.Interfaces;
using BookyPets.Domain.BookAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Readers.Queries.GetReaderProgresses;

public class GetReaderProgressesQueryHandler(
    ICurrentReaderProvider currentReaderProvider,
    IReadersRepository readersRepository,
    IProgressesRepository progressesRepository) : IHandler<GetReaderProgressesQuery, Result<List<Progress>>>
{
    private readonly IReadersRepository _readersRepository = readersRepository;
    private readonly IProgressesRepository progressesRepository = progressesRepository;
    private readonly ICurrentReaderProvider _currentReaderProvider = currentReaderProvider;

    public async Task<Result<List<Progress>>> HandleAsync(GetReaderProgressesQuery query, CancellationToken cancellationToken = default)
    {
        var currentReader = _currentReaderProvider.GetCurrentReader();

        var reader = await _readersRepository.GetByIdAsync(currentReader.Id);

        if (reader is null)
            return new Error(ErrorType.NotFound, "ReaderNotFound", "Reader was not found");

        var currentProgressIds = reader.GetProgresses();

        var currentProgresses = await progressesRepository.GetProgressesAsync(currentProgressIds);

        return currentProgresses;
    }
}
