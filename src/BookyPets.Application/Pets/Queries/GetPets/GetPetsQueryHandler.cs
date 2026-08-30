using BookyPets.Application.Common.Interfaces;
using BookyPets.Domain.PetAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Pets.Queries.GetPets;

public class GetPetsQueryHandler(IPetsRepository petsRepository) : IHandler<GetPetsQuery, Result<List<Pet>>>
{
    private readonly IPetsRepository _petsRepository = petsRepository;

    public async Task<Result<List<Pet>>> HandleAsync(GetPetsQuery request, CancellationToken cancellationToken = default)
    {
        var pets = await _petsRepository.GetPetsAsync(request.Search);

        return pets;
    }
}

