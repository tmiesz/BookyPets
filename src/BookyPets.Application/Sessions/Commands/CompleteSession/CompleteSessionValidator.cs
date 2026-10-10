using BookyPets.Shared.Validator;

namespace BookyPets.Application.Sessions.Commands.CompleteSession;

public class CompleteSessionCommandValidator : AbstractValidator<CompleteSessionCommand>
{
    public CompleteSessionCommandValidator()
    {
        RuleFor(x => x.PagesRead)
            .GreaterThanOrEqualTo(0);
    }
}
