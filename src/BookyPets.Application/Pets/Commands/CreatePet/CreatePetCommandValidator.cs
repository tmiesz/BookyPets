using BookyPets.Shared.Validator;

namespace BookyPets.Application.Pets.Commands.CreatePet;

public class CreatePetCommandValidator : AbstractValidator<CreatePetCommand>
{
    public CreatePetCommandValidator()
    {
        RuleFor(x => x.Name)
            .MinLength(3)
            .MaxLength(20);
    }
}
