using BookyPets.Shared.Validator;

namespace BookyPets.Application.Books.Queries.SearchBooks;

public class SearchBooksQueryValidator : AbstractValidator<SearchBooksQuery>
{
    public SearchBooksQueryValidator()
    {
        RuleFor(x => x.Limit)
            .GreaterThan(0)
            .LessThan(51);
    }
}
