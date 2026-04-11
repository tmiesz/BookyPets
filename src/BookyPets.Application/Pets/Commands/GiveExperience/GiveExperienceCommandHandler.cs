using BookyPets.Application.Common.Interfaces;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Pets.Commands.GiveExperience;

public class GiveExperienceCommandHandler : IHandler<GiveExperienceCommand, Result>
{
    private readonly IPetsRepository _petsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GiveExperienceCommandHandler(IPetsRepository petsRepository, IUnitOfWork unitOfWork)
    {
        _petsRepository = petsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(GiveExperienceCommand request, CancellationToken cancellationToken = default)
    {
        var pet = await _petsRepository.GetPetAsync(request.PetId);

        if (pet is null)
            return new Error(ErrorType.NotFound, "PetNotFound", "Pet was not found");

        var result = pet.GainExperience(request.Experience);

        if (!result.IsSuccess)
            return result;

        await _petsRepository.UpdatePetAsync(pet);
        await _unitOfWork.CommitChangesAsync();

        return result;
    }
}
