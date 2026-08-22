namespace BookyPets.Shared.Result;

public class Error(ErrorType type, string? code = null, string? description = null)
{
    public ErrorType Type { get; } = type;
    public string Code { get; } = code ?? type.ToString();
    public string Description { get; } = description ?? type.ToString();
}

public enum ErrorType
{
    NotFound,
    Validation,
    Conflict,
    Unauthorized,
    Forbidden,
    Failure
}
