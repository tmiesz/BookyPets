using BookyPets.Application.Common.Interfaces;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Readers.Commands.AcquirePet;

public class AcquirePetCommandHandler : IHandler<AcquirePetCommand, Result>
{
    private readonly IPetsRepository _petsRepository;
    private readonly IReadersRepository _readersRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentReaderProvider _currentReaderProvider;

    public AcquirePetCommandHandler(IPetsRepository petsRepository, IReadersRepository readersRepository, IUnitOfWork unitOfWork, ICurrentReaderProvider currentReaderProvider)
    {
        _petsRepository = petsRepository;
        _readersRepository = readersRepository;
        _unitOfWork = unitOfWork;
        _currentReaderProvider = currentReaderProvider;
    }

    public async Task<Result> HandleAsync(AcquirePetCommand request, CancellationToken cancellationToken = default)
    {
        var currentReader = _currentReaderProvider.GetCurrentReader();

        var pet = await _petsRepository.GetPetAsync(request.PetId);

        if (pet is null)
            return new Error(ErrorType.NotFound, "PetNotFound", "Pet was not found");

        var reader = await _readersRepository.GetByIdAsync(currentReader.Id);

        if (reader is null)
            return new Error(ErrorType.NotFound, "ReaderNotFound", "Reader was not found");

        var result = reader.AcquirePet(pet.Id);

        if (!result.IsSuccess)
            return result;

        await _readersRepository.UpdateReaderAsync(reader);
        await _unitOfWork.CommitChangesAsync();

        return result;
    }
}
