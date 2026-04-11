using BookyPets.Application.Authentication.Common;
using BookyPets.Application.Common.Interfaces;
using BookyPets.Domain.Common.Interfaces;
using BookyPets.Domain.ReaderAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Authentication.Commands;

public class RegisterCommandHandler(
    IJwtTokenGenerator _jwtTokenGenerator,
    IPasswordHasher _passwordHasher,
    IReadersRepository _readersRepository,
    IUnitOfWork _unitOfWork)
        : IHandler<RegisterCommand, Result<AuthenticationResult>>
{
    public async Task<Result<AuthenticationResult>> HandleAsync(RegisterCommand command, CancellationToken cancellationToken = default)
    {
        if (await _readersRepository.ExistsByEmailAsync(command.Email))
        {
            return new Error(ErrorType.Conflict, "User exists", "User with provided email already exists.");
        }

        var hashPasswordResult = _passwordHasher.HashPassword(command.Password);

        if (!hashPasswordResult.IsSuccess)
        {
            return hashPasswordResult.Error;
        }

        var reader = new Reader(
            command.FirstName,
            command.LastName,
            command.Email,
            hashPasswordResult.Value);

        await _readersRepository.AddReaderAsync(reader);
        await _unitOfWork.CommitChangesAsync();

        var token = _jwtTokenGenerator.GenerateToken(reader);

        return new AuthenticationResult(reader, token);

    }
}
