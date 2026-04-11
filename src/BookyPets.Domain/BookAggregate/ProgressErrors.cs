using BookyPets.Shared.Result;
namespace BookyPets.Domain.BookAggregate;

public static class ProgressErrors
{
    public static readonly Error TooManyPages = new(
        ErrorType.Failure,
        "Book.TooManyPages",
        "The book doesn't have that many pages.");

    public static readonly Error InvalidPage = new(
        ErrorType.Failure,
        "Book.InvalidPage",
        "Page number must be greater than zero.");

    public static readonly Error CannotGoBackwards = new(
        ErrorType.Failure,
        "Book.CannotGoBackwards",
        "Cannot update to a page earlier than current progress.");

    public static readonly Error AlreadyCompleted = new(
        ErrorType.Failure,
        "Book.AlreadyCompleted",
        "The book is already completed.");

    public static readonly Error CannotManuallyComplete = new(
        ErrorType.Failure,
        "Book.CannotManuallyComplete",
        "Books can only be completed by reading to the final page.");
}
