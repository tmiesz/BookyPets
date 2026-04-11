namespace BookyPets.Shared.Validator;

public class ValidationResult
{
    public IReadOnlyList<ValidationFailure> Failures { get; }
    public bool IsValid => Failures.Count == 0;

    public ValidationResult()
    {
        Failures = [];
    }

    public ValidationResult(IEnumerable<ValidationFailure> failures)
    {
        Failures = (failures ?? []).ToList().AsReadOnly();
    }

    public override string ToString() => IsValid ? "Valid" : string.Join(Environment.NewLine, Failures);
}
