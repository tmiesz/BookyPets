using BookyPets.Application.Common.Interfaces;
using BookyPets.Domain.ReaderAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Readers.Queries.GetReader;

public class GetReaderQueryHandler : IHandler<GetReaderQuery, Result<Reader>>
{
    private readonly IReadersRepository _readersRepository;

    public GetReaderQueryHandler(IReadersRepository readersRepository)
    {
        _readersRepository = readersRepository;
    }

    public async Task<Result<Reader>> HandleAsync(GetReaderQuery query, CancellationToken cancellationToken = default)
    {
        var reader = await _readersRepository.GetByIdAsync(query.ReaderId);

        if (reader is null)
            return new Error(ErrorType.NotFound, "ReaderNotFound");

        return reader;
    }
}
