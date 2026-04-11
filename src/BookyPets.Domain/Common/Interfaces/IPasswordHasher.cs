using BookyPets.Shared.Result;

namespace BookyPets.Domain.Common.Interfaces;

public interface IPasswordHasher
{
    public Result<string> HashPassword(string password);
    bool IsCorrectPassword(string password, string hash);
}
