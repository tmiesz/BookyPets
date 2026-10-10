using System.Net.Mail;
using BookyPets.Shared.Validator;

namespace BookyPets.Application.Authentication.Commands;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaxLength(50);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaxLength(50);

        RuleFor(x => x.Email)
            .NotEmpty()
            .MaxLength(254)
            .Must(IsValidEmail).WithMessage("Value must be a valid email address");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MaxLength(128);
    }

    private static bool IsValidEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        var trimmed = email.Trim();

        return MailAddress.TryCreate(trimmed, out var address)
            && address.Address == trimmed
            && address.Host.Contains('.');
    }
}
