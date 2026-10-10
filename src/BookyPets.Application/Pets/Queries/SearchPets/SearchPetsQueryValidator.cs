using BookyPets.Shared.Validator;

namespace BookyPets.Application.Pets.Queries.SearchPets;

public class SearchPetsQueryValidator : AbstractValidator<SearchPetsQuery>
{
    public SearchPetsQueryValidator()
    {
        RuleFor(x => x.Limit)
            .GreaterThan(0)
            .LessThan(101);

        RuleFor(x => x.Offset)
            .GreaterThan(-1).WithMessage("Value must be zero or greater");
    }
}
