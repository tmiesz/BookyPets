using BookyPets.Application.Authentication.Common;
using BookyPets.Application.Common.Interfaces;
using BookyPets.Domain.Common.Interfaces;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;

namespace BookyPets.Application.Authentication.Queries;

public class LoginQueryHandler(
    IJwtTokenGenerator _jwtTokenGenerator,
    IPasswordHasher _passwordHasher,
    IReadersRepository _readersRepository) : IHandler<LoginQuery, Result<AuthenticationResult>>
{
    public async Task<Result<AuthenticationResult>> HandleAsync(LoginQuery query, CancellationToken cancellationToken = default)
    {
        var reader = await _readersRepository.GetByEmailAsync(query.Email);

        return reader is null || !reader.IsCorrectPasswordHash(query.Password, _passwordHasher)
            ? AuthenticationErrors.InvalidCredentials
            : new AuthenticationResult(reader, _jwtTokenGenerator.GenerateToken(reader));
    }
}
