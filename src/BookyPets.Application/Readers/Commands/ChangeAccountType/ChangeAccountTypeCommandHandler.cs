using BookyPets.Application.Common.Interfaces;
using BookyPets.Domain.ReaderAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Readers.Commands.ChangeAccountType;

public class ChangeAccountTypeCommandHandler : IHandler<ChangeAccountTypeCommand, Result<Reader>>
{
    private readonly IReadersRepository _readersRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeAccountTypeCommandHandler(IReadersRepository readersRepository, IUnitOfWork unitOfWork)
    {
        _readersRepository = readersRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Reader>> HandleAsync(ChangeAccountTypeCommand request, CancellationToken cancellationToken = default)
    {
        var reader = await _readersRepository.GetByIdAsync(request.ReaderId);

        if (reader is null)
            return new Error(ErrorType.NotFound, "ReaderNotFound", "Reader was not found");

        var changeAccountTypeResult = reader.ChangeAccountType(request.AccountType);

        if (!changeAccountTypeResult.IsSuccess)
        {
            return changeAccountTypeResult.Error;
        }

        await _readersRepository.UpdateReaderAsync(reader);
        await _unitOfWork.CommitChangesAsync();
        return reader;
    }
}
