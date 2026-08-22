namespace BookyPets.Shared.Result;

public class Result : IResult
{
    protected readonly Error? _error;
    public bool IsSuccess => _error is null;
    public Error Error => _error ?? throw new InvalidOperationException("Result is a success");

    protected Result(Error error) { _error = error; }
    protected Result() { }

    public static readonly Result Success = new();
    public static implicit operator Result(Error error) => new(error);

    public TResult Match<TResult>(Func<TResult> success, Func<Error, TResult> failure)
    {
        return IsSuccess ? success() : failure(_error!);
    }
}

public class Result<T> : Result
{
    private readonly T _value = default!;

    public T Value => IsSuccess ? _value : throw new InvalidOperationException("Result is an error");

    private Result(T value) { _value = value; }
    private Result(Error error) : base(error) { }

    public static implicit operator Result<T>(T value) => new(value);
    public static implicit operator Result<T>(Error error) => new(error);

    public TResult Match<TResult>(Func<T, TResult> success, Func<Error, TResult> failure)
    {
        return IsSuccess ? success(_value!) : failure(_error!);
    }
}
