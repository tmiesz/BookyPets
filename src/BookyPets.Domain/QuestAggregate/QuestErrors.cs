using BookyPets.Shared.Result;

namespace BookyPets.Domain.QuestAggregate;

public static class QuestErrors
{
    public static readonly Error AlreadyCompleted = new(
        ErrorType.Failure,
        "Quest.Complete",
        "Quest is already completed.");

    public static readonly Error NotActive = new(
        ErrorType.Failure,
        "Quest.Complete",
        "Quest is not active.");
}
