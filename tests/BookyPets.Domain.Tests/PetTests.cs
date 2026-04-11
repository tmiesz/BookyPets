using BookyPets.Domain.PetAggregate;
using Common.Tests.Pets;

namespace BookyPets.Domain.Tests;

public class PetTests
{
    [Fact]
    public void GainExperience_WhenEnough_ShouldLevel()
    {
        var pet = PetFactory.CreatePet();

        var result = pet.GainExperience(1500);

        Assert.True(result.IsSuccess);
        Assert.NotEqual(1, pet.Level);
    }


    [Fact]
    public void GainExperience_WhenNegative_ShouldFail()
    {
        var pet = PetFactory.CreatePet();

        var result = pet.GainExperience(-1);

        Assert.False(result.IsSuccess);
        Assert.Equal(PetErrors.InvalidExperience.Code, result.Error.Code);
    }
}
