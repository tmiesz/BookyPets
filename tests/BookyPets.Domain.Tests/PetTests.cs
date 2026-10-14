using BookyPets.Domain.PetAggregate;
using Common.Tests.Pets;

namespace BookyPets.Domain.Tests;

public class PetTests
{
    [Fact]
    public void GainExperience_WhenExactlyEnoughForFiveLevels_ShouldReachLevelSixWithNoExperienceLeft()
    {
        var pet = PetFactory.CreatePet();

        var result = pet.GainExperience(1500);

        Assert.True(result.IsSuccess);
        Assert.Equal(6, pet.Level);
        Assert.Equal(0, pet.Experience);
    }

    [Theory]
    [InlineData(0, 1, 0)]
    [InlineData(99, 1, 99)]
    [InlineData(100, 2, 0)]
    [InlineData(101, 2, 1)]
    [InlineData(299, 2, 199)]
    [InlineData(300, 3, 0)]
    public void GainExperience_AtLevelBoundaries_ShouldSetExpectedLevelAndExperience(
        int experience,
        int expectedLevel,
        int expectedExperience)
    {
        var pet = PetFactory.CreatePet();

        var result = pet.GainExperience(experience);

        Assert.True(result.IsSuccess);
        Assert.Equal(expectedLevel, pet.Level);
        Assert.Equal(expectedExperience, pet.Experience);
    }

    [Fact]
    public void GainExperience_WhenGivenInTwoSteps_ShouldCarryExperienceOver()
    {
        var pet = PetFactory.CreatePet();

        pet.GainExperience(60);
        pet.GainExperience(60);

        Assert.Equal(2, pet.Level);
        Assert.Equal(20, pet.Experience);
    }

    [Fact]
    public void ExperienceForNextLevel_AfterLevellingUp_ShouldGrowWithLevel()
    {
        var pet = PetFactory.CreatePet();

        Assert.Equal(100, pet.ExperienceForNextLevel);

        pet.GainExperience(100);

        Assert.Equal(200, pet.ExperienceForNextLevel);
    }

    [Fact]
    public void GainExperience_WhenNegative_ShouldFailAndKeepState()
    {
        var pet = PetFactory.CreatePet();

        var result = pet.GainExperience(-1);

        Assert.False(result.IsSuccess);
        Assert.Equal(PetErrors.InvalidExperience.Code, result.Error.Code);
        Assert.Equal(1, pet.Level);
        Assert.Equal(0, pet.Experience);
    }
}
