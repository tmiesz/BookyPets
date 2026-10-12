namespace BookyPets.Infrastructure.Authentication.TokenGenerator;

public class JwtSettings
{
    public const string Section = "JwtSettings";

    /// <summary>
    /// The public placeholder shipped in flake.nix / the README. Accepted in Development only.
    /// </summary>
    public const string DevelopmentPlaceholderSecret = "random-string-at-least-32-characters-long";

    public const int MinimumSecretLength = 32;

    public string Audience { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Secret { get; set; } = null!;
    public int TokenExpirationInMinutes { get; set; }
}
