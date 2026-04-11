using BookyPets.Shared.SmartEnum;

namespace BookyPets.Domain.BookAggregate;

public class Genre : SmartEnum<Genre>
{
    public static readonly Genre Science = new(nameof(Science), 0);
    public static readonly Genre Technology = new(nameof(Technology), 1);
    public static readonly Genre Philosophy = new(nameof(Philosophy), 2);
    public static readonly Genre History = new(nameof(History), 3);
    public static readonly Genre Psychology = new(nameof(Psychology), 4);
    public static readonly Genre Fiction = new(nameof(Fiction), 5);
    public static readonly Genre Fantasy = new(nameof(Fantasy), 6);
    public static readonly Genre Biography = new(nameof(Biography), 7);
    public static readonly Genre Educational = new(nameof(Educational), 8);

    public Genre(string name, int value) : base(name, value)
    {

    }
}
