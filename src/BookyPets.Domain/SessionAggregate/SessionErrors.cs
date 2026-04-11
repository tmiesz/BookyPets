using BookyPets.Shared.Result;

namespace BookyPets.Domain.SessionAggregate;

public static class SessionErrors
{
    public static readonly Error SessionNotActive = new(
        ErrorType.Failure,
        "Session.NotActive",
        "Session is not active.");

    public static readonly Error InvalidPagesRead = new(
        ErrorType.Failure,
        "Session.InvalidPagesRead",
        "Pages read cannot be negative.");
}
