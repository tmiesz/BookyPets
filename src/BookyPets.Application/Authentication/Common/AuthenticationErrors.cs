using BookyPets.Shared.Result;

namespace BookyPets.Application.Authentication.Common;

public static class AuthenticationErrors
{
    public static readonly Error InvalidCredentials = new(
        ErrorType.Unauthorized,
        "Authentication.InvalidCredentails",
        "Invalid Credentials.");
}
