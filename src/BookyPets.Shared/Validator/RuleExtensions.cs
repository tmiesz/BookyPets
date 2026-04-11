namespace BookyPets.Shared.Validator;

public static class RuleExtensions
{
    public static PropertyRule<T, string> NotNull<T>(
        this PropertyRule<T, string> rule)
        => rule.AddRule(
            v => v is not null,
            _ => "Value must not be null");

    public static PropertyRule<T, string> NotEmpty<T>(
        this PropertyRule<T, string> rule)
        => rule.AddRule(
            v => !string.IsNullOrWhiteSpace(v),
            _ => "Value must not be empty");

    public static PropertyRule<T, string> MinLength<T>(
        this PropertyRule<T, string> rule, int min)
        => rule.AddRule(
            v => v is not null && v.Length >= min,
            v => $"Value length must be greater than {min}");

    public static PropertyRule<T, string> MaxLength<T>(
        this PropertyRule<T, string> rule, int max)
        => rule.AddRule(
            v => v is not null && v.Length <= max,
            v => $"Value length must be less than {max}");

    public static PropertyRule<T, int> GreaterThan<T>(
        this PropertyRule<T, int> rule, int threshold)
        => rule.AddRule(
            v => v > threshold,
            v => $"Value must be greater than {threshold}");

    public static PropertyRule<T, int> LessThan<T>(
        this PropertyRule<T, int> rule, int threshold)
        => rule.AddRule(
            v => v < threshold,
            v => $"Value must be less than {threshold}");

    public static PropertyRule<T, TProperty> Must<T, TProperty>(
        this PropertyRule<T, TProperty> rule,
        Func<TProperty, bool> predicate)
        => rule.AddRule(predicate, _ => "Value did not satisfy the rule");
}
