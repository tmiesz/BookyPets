using System.Reflection;

namespace BookyPets.Shared.SmartEnum;

public abstract class SmartEnum<TEnum> : IEquatable<SmartEnum<TEnum>>
   where TEnum : SmartEnum<TEnum>
{
    private static readonly Dictionary<int, TEnum> Enumerations = CreateEnumerations();

    protected SmartEnum(string name, int value)
    {
        Name = name;
        Value = value;
    }

    public string Name { get; protected init; } = string.Empty;
    public int Value { get; protected init; }

    public static TEnum? FromValue(int value)
    {
        return Enumerations.TryGetValue(value, out TEnum? enumeration) ?
            enumeration :
            default;
    }

    public static TEnum? FromName(string name)
    {
        return Enumerations.Values.SingleOrDefault(e => e.Name == name);
    }

    public static bool TryFromName(string name, out TEnum? result)
    {
        result = Enumerations.Values.SingleOrDefault(e => e.Name == name);
        return result is not null;
    }

    public bool Equals(SmartEnum<TEnum>? other)
    {
        if (other is null)
            return false;

        return GetType() == other.GetType() &&
            Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is SmartEnum<TEnum> other &&
            Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    private static Dictionary<int, TEnum> CreateEnumerations()
    {
        var enumType = typeof(TEnum);

        var fieldsForType = enumType
            .GetFields(
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.FlattenHierarchy)
            .Where(fieldInfo =>
                enumType.IsAssignableFrom(fieldInfo.FieldType))
            .Select(fieldInfo =>
                (TEnum)fieldInfo.GetValue(default)!);

        return fieldsForType.ToDictionary(x => x.Value);
    }
}
