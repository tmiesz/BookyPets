using BookyPets.Application.Common.Interfaces;
using BookyPets.Domain.PetAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Readers.Queries.GetReaderPets;

public class GetReaderPetsQueryHandler(
    ICurrentReaderProvider currentReaderProvider,
    IReadersRepository readersRepository,
    IPetsRepository petsRepository) : IHandler<GetReaderPetsQuery, Result<List<Pet>>>
{
    private readonly IReadersRepository _readersRepository = readersRepository;
    private readonly IPetsRepository _petsRepository = petsRepository;
    private readonly ICurrentReaderProvider _currentReaderProvider = currentReaderProvider;

    public async Task<Result<List<Pet>>> HandleAsync(GetReaderPetsQuery query, CancellationToken cancellationToken = default)
    {
        var currentReader = _currentReaderProvider.GetCurrentReader();

        var reader = await _readersRepository.GetByIdAsync(currentReader.Id);

        if (reader is null)
            return new Error(ErrorType.NotFound, "ReaderNotFound", "Reader was not found");

        var ownedPetIds = reader.GetPets();

        var ownedPets = await _petsRepository.GetPetsAsync(ownedPetIds);

        return ownedPets;
    }
}
