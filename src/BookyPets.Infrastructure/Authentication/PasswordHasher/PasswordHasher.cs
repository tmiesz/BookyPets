using System.Text.RegularExpressions;
using BookyPets.Domain.Common.Interfaces;
using BookyPets.Shared.Result;

namespace BookyPets.Infrastructure.Authentication.PasswordHasher;

public partial class PasswordHasher : IPasswordHasher
{
    private static readonly Regex PasswordRegex = StringPasswordRegex();

    [GeneratedRegex("^(?=.*[A-Z].*[A-Z])(?=.*[!@#$&*])(?=.*[0-9].*[0-9])(?=.*[a-z].*[a-z].*[a-z]).{8,}$")]
    private static partial Regex StringPasswordRegex();

    public Result<string> HashPassword(string password)
    {
        return !PasswordRegex.IsMatch(password)
            ? new Error(ErrorType.Validation, description: "Password too weak")
            : BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    }

    public bool IsCorrectPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
    }
}
