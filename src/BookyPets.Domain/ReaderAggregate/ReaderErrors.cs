using BookyPets.Shared.Result;

namespace BookyPets.Domain.ReaderAggregate;

public static class ReaderErrors
{
    public static readonly Error InvalidName = new(
        ErrorType.Failure,
        "Reader.InvalidName",
        "Name cannot be empty or whitespace.");

    public static readonly Error PetLimitReached = new(
        ErrorType.Failure,
        "Reader.PetLimitReached",
        "Maximum number of pets reached for this account type.");

    public static readonly Error PetAlreadyOwned = new(
        ErrorType.Failure,
        "Reader.PetAlreadyOwned",
        "You already own this pet.");

    public static readonly Error BookAlreadyOwned = new(
        ErrorType.Failure,
        "Reader.BookAlreadyOwned",
        "You already own this book.");

    public static readonly Error QuestAlreadyActive = new(
        ErrorType.Failure,
        "Reader.QuestAlreadyActive",
        "You already own this quest.");

    public static readonly Error QuestLimitReached = new(
        ErrorType.Failure,
        "Reader.QuestLimitReached",
        "Maximum number of active quests reached for this account type.");

    public static readonly Error TooManyPetsForDowngrade = new(
        ErrorType.Failure,
        "Reader.TooManyPetsForDowngrade",
        "Cannot downgrade account: you own more pets than the new tier allows.");

    public static readonly Error TooManyQuestsForDowngrade = new(
        ErrorType.Failure,
        "Reader.TooManyQuestsForDowngrade",
        "Cannot downgrade account: you have more active quests than the new tier allows.");

    public static readonly Error SessionAlreadyActive = new(
        ErrorType.Failure,
        "Reader.SessionAlreadyActive",
        "A reading session is already in progress.");
}
