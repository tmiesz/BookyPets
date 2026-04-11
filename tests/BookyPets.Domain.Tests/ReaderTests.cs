using BookyPets.Domain.ReaderAggregate;
using Common.Tests.Readers;

namespace BookyPets.Domain.Tests;

public class ReaderTests
{
    [Fact]
    public void AcquirePet_WhenMaxPetsOwned_FailsWithPetLimitReached()
    {
        var reader = ReaderFactory.CreateReader();

        for (int i = 0; i < 5; i++)
        {
            reader.AcquirePet(Guid.NewGuid());
        }

        var result = reader.AcquirePet(Guid.NewGuid());

        Assert.False(result.IsSuccess);
        Assert.Equal(ReaderErrors.PetLimitReached.Code, result.Error.Code);
    }

    [Fact]
    public void ChangeAccountType_WhenPetsExceedNewLimit_FailsWithTooManyPetsForDowngrade()
    {
        var reader = ReaderFactory.CreateProReader();

        for (int i = 0; i < 10; i++)
        {
            reader.AcquirePet(Guid.NewGuid());
        }

        var result = reader.ChangeAccountType(AccountType.Free);

        Assert.False(result.IsSuccess);
        Assert.Equal(ReaderErrors.TooManyPetsForDowngrade.Code, result.Error.Code);
    }
}
