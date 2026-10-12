using BookyPets.Shared.Validator;

namespace BookyPets.Application.Readers.Queries.GetReaderLibrary;

public class GetReaderLibraryQueryValidator : AbstractValidator<GetReaderLibraryQuery>
{
    public GetReaderLibraryQueryValidator()
    {
        RuleFor(x => x.Limit)
            .GreaterThan(0)
            .LessThan(101);

        RuleFor(x => x.Offset)
            .GreaterThan(-1).WithMessage("Value must be zero or greater");
    }
}
