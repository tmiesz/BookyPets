using BookyPets.Shared.SmartEnum;

namespace BookyPets.Domain.ReaderAggregate;

public class AccountType : SmartEnum<AccountType>
{
    public static readonly AccountType Free = new(nameof(Free), 0);
    public static readonly AccountType Basic = new(nameof(Basic), 1);
    public static readonly AccountType Pro = new(nameof(Pro), 2);

    public AccountType(string name, int value) : base(name, value)
    {

    }

    public int MaxOwnedPets() => Name switch
    {
        nameof(Free) => 5,
        nameof(Basic) => 15,
        nameof(Pro) => 50,
        _ => throw new InvalidOperationException()
    };

    public int MaxActiveQuests() => Name switch
    {
        nameof(Free) => 1,
        nameof(Basic) => 5,
        nameof(Pro) => int.MaxValue,
        _ => throw new InvalidOperationException()
    };

    public int MaxDailySessions() => Name switch
    {
        nameof(Free) => 5,
        nameof(Basic) => int.MaxValue,
        nameof(Pro) => int.MaxValue,
        _ => throw new InvalidOperationException()
    };

    public IReadOnlyList<string> GetPermissions() => Name switch
    {
        nameof(Free) => ["books:acquire", "pets:acquire", "sessions:start", "sessions:finish"],
        nameof(Basic) => ["books:acquire", "pets:acquire", "sessions:start", "sessions:finish"],
        nameof(Pro) => ["books:acquire", "pets:acquire", "sessions:start", "sessions:finish"],
        _ => throw new InvalidOperationException()
    };
}
