namespace BookyPets.Shared.Result;

public class Error
{
    public ErrorType Type { get; }
    public string Code { get; }
    public string Description { get; }

    public Error(ErrorType type, string? code = null, string? description = null)
    {
        Type = type;
        Code = code ?? type.ToString();
        Description = description ?? type.ToString();
    }
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
