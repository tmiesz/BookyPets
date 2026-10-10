using BookyPets.Shared.Validator;

namespace BookyPets.Application.Pets.Commands.GiveExperience;

public class GiveExperienceCommandValidator : AbstractValidator<GiveExperienceCommand>
{
    public GiveExperienceCommandValidator()
    {
        RuleFor(x => x.Experience)
            .GreaterThanOrEqualTo(0);
    }
}
