using System.Linq.Expressions;

namespace BookyPets.Shared.Validator;

public abstract class AbstractValidator<T> : IValidator<T>
{
    private readonly List<Func<T, IEnumerable<ValidationFailure>>> _rules = new();

    protected PropertyRule<T, TProperty> RuleFor<TProperty>(Expression<Func<T,TProperty>> expression)
    {
        var rule = new PropertyRule<T, TProperty>(expression);

        _rules.Add(instance=>rule.Validate(instance));

        return rule;
    }

    public ValidationResult Validate(T instance)
    {
        ArgumentNullException.ThrowIfNull(instance);

        var failures = _rules.SelectMany(rule => rule(instance));

        return new ValidationResult(failures);
    }
}
