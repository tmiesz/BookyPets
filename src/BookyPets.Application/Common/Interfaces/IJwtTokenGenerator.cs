using BookyPets.Domain.ReaderAggregate;

namespace BookyPets.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(Reader reader);
}
