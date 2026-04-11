using BookyPets.Domain.BookAggregate;
using BookyPets.Shared.Validator;

namespace BookyPets.Application.Books.Commands;

public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator()
    {
        RuleFor(x => x.Title)
            .MinLength(3)
            .MaxLength(100);

        RuleFor(x => x.Author)
            .MinLength(3)
            .MaxLength(50);

        RuleFor(x => x.PageCount)
            .GreaterThan(0);

        RuleFor(x => x.Genre)
            .Must(g => g is not null && Genre.FromName(g.Name) is not null);
    }
}
