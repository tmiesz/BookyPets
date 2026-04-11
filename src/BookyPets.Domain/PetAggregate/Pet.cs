using BookyPets.Domain.BookAggregate;
using BookyPets.Domain.Common;
using BookyPets.Shared.Result;

namespace BookyPets.Domain.PetAggregate;

public class Pet : AggregateRoot
{
    public string Name { get; private set; }
    public Genre? FavouriteGenre { get; private set; }
    private int _experience;
    public int Level { get; private set; }

    private Pet()
    {
        Name = null!;
    }

    public Pet(string name, Genre? favouriteGenre, Guid? id = null) : base(id ?? Guid.NewGuid())
    {
        Name = name;
        FavouriteGenre = favouriteGenre;
        Level = 1;
    }

    public Result GainExperience(int experience)
    {
        if (experience < 0)
            return PetErrors.InvalidExperience;

        _experience += experience;

        while (_experience >= ExperienceForNextLevel())
        {
            _experience -= ExperienceForNextLevel();
            Level++;
        }

        return Result.Success;
    }

    public Result GainExperienceFromSession(int pagesRead, int minutesRead, Genre sessionGenre)
    {
        var exp = 10 + pagesRead * 5 + Math.Min(minutesRead, pagesRead * 3);

        if (FavouriteGenre is not null && FavouriteGenre.Equals(sessionGenre))
            exp = (int)(exp * 1.5);

        return GainExperience(exp);
    }

    private int ExperienceForNextLevel() => Level * 100;
}
