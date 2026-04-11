using BookyPets.Shared.SmartEnum;

namespace BookyPets.Domain.BookAggregate;

public class BookStatus : SmartEnum<BookStatus>
{
    public static readonly BookStatus Reading = new(nameof(Reading), 0);
    public static readonly BookStatus Completed = new(nameof(Completed), 1);
    public static readonly BookStatus OnHold = new(nameof(OnHold), 2);
    public static readonly BookStatus Dropped = new(nameof(Dropped), 3);
    public static readonly BookStatus PlanToRead = new(nameof(PlanToRead), 4);

    public BookStatus(string name, int value) : base(name, value)
    {
    }
}
