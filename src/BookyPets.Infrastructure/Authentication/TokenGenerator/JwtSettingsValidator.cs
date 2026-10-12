using Microsoft.Extensions.Options;

namespace BookyPets.Infrastructure.Authentication.TokenGenerator;

public class JwtSettingsValidator(bool isDevelopment) : IValidateOptions<JwtSettings>
{
    public ValidateOptionsResult Validate(string? name, JwtSettings options)
    {
        var failures = new List<string>();

        if (string.IsNullOrWhiteSpace(options.Secret) || options.Secret.Length < JwtSettings.MinimumSecretLength)
        {
            failures.Add($"{JwtSettings.Section}:Secret must be at least {JwtSettings.MinimumSecretLength} characters long.");
        }
        else if (!isDevelopment && options.Secret == JwtSettings.DevelopmentPlaceholderSecret)
        {
            failures.Add($"{JwtSettings.Section}:Secret is the public development placeholder and cannot be used outside Development.");
        }

        if (options.TokenExpirationInMinutes <= 0)
        {
            failures.Add($"{JwtSettings.Section}:TokenExpirationInMinutes must be greater than 0.");
        }

        if (string.IsNullOrWhiteSpace(options.Issuer))
        {
            failures.Add($"{JwtSettings.Section}:Issuer must not be empty.");
        }

        if (string.IsNullOrWhiteSpace(options.Audience))
        {
            failures.Add($"{JwtSettings.Section}:Audience must not be empty.");
        }

        return failures.Count == 0
            ? ValidateOptionsResult.Success
            : ValidateOptionsResult.Fail(failures);
    }
}
