using BookyPets.Application.Common.Interfaces;
using BookyPets.Domain.PetAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Pets.Commands.CreatePet;

public class CreatePetCommandHandler : IHandler<CreatePetCommand, Result<Pet>>
{
    private readonly IPetsRepository _petsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePetCommandHandler(IPetsRepository petsRepository, IUnitOfWork unitOfWork)
    {
        _petsRepository = petsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Pet>> HandleAsync(CreatePetCommand request, CancellationToken cancellationToken = default)
    {
        var pet = new Pet(name: request.Name, favouriteGenre: request.Genre);

        await _petsRepository.AddPetAsync(pet);
        await _unitOfWork.CommitChangesAsync();

        return pet;
    }
}
