namespace BookyPets.Shared.Validator;

public interface IValidator<T>
{
    ValidationResult Validate(T instance);
}
