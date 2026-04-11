using System.Linq.Expressions;

namespace BookyPets.Shared.Validator;

public class PropertyRule<T, TProperty>
{
    private readonly string _propertyName;
    private readonly Func<T, TProperty> _propertySelector;
    private readonly List<(Func<TProperty, bool> Predicate, Func<TProperty, string> MessageFactory)> _rules = new();
    private string? _pendingMessage;

    public PropertyRule(Expression<Func<T, TProperty>> expression)
    {
        _propertyName = GetPropertyName(expression);
        _propertySelector = expression.Compile();
    }

    internal PropertyRule<T, TProperty> AddRule(
        Func<TProperty, bool> predicate,
        Func<TProperty, string> defaultMessage)
    {
        var message = _pendingMessage is not null
            ? _ => _pendingMessage
            : defaultMessage;

        _pendingMessage = null;
        _rules.Add((predicate, message));

        return this;
    }

    public PropertyRule<T, TProperty> WithMessage(string message)
    {
        if (_rules.Count == 0)
            throw new InvalidOperationException("Call a rule first");

        var last = _rules[^1];
        _rules[^1] = (last.Predicate, _ => message);

        return this;
    }

    internal IEnumerable<ValidationFailure> Validate(T instance)
    {
        var value = _propertySelector(instance);

        foreach (var (predicate, messageFactory) in _rules)
        {
            if (!predicate(value))
                yield return new ValidationFailure(_propertyName, messageFactory(value));
        }
    }

    private static string GetPropertyName(Expression<Func<T, TProperty>> expression)
    {
        if (expression.Body is MemberExpression member)
            return member.Member.Name;

        throw new ArgumentException("Simplify property accessor");
    }
}
