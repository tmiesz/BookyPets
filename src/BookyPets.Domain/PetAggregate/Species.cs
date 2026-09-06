using BookyPets.Shared.SmartEnum;

namespace BookyPets.Domain.PetAggregate;

public class Species(string name, int value) : SmartEnum<Species>(name, value)
{
    public static readonly Species Bear = new(nameof(Bear), 0);
    public static readonly Species Bee = new(nameof(Bee), 1);
    public static readonly Species Bird = new(nameof(Bird), 2);
    public static readonly Species Butterfly = new(nameof(Butterfly), 3);
    public static readonly Species Dog = new(nameof(Dog), 4);
    public static readonly Species Lion = new(nameof(Lion), 5);
    public static readonly Species Tiger = new(nameof(Tiger), 6);
    public static readonly Species Whale = new(nameof(Whale), 7);
}
