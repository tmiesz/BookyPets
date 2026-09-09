using BookyPets.Application.Common.Interfaces;
using BookyPets.Domain.PetAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Pets.Queries.SearchPets;

public class SearchPetsQueryHandler(IPetsRepository petsRepository) : IHandler<SearchPetsQuery, Result<List<Pet>>>
{
    private readonly IPetsRepository _petsRepository = petsRepository;

    public async Task<Result<List<Pet>>> HandleAsync(SearchPetsQuery request, CancellationToken cancellationToken = default)
    {
        var pets = await _petsRepository.SearchPetsAsync(request.Search);

        return pets;
    }
}

