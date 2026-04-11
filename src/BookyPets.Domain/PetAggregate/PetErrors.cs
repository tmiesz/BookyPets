using BookyPets.Shared.Result;

namespace BookyPets.Domain.PetAggregate;

public static class PetErrors
{
    public static readonly Error InvalidExperience = new(
        ErrorType.Failure,
        "Pet.Experience",
        "Cannot give negative experience to pets.");
}
