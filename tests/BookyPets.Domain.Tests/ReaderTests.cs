using BookyPets.Domain.BookAggregate.Events;
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

    [Theory]
    [InlineData("Free", 5)]
    [InlineData("Basic", 15)]
    [InlineData("Pro", 50)]
    public void AcquirePet_AtAccountTypeLimit_AllowsTheLimitAndRejectsTheNextPet(string accountTypeName, int limit)
    {
        var reader = ReaderFactory.CreateReader();
        Assert.True(reader.ChangeAccountType(AccountType.FromName(accountTypeName)!).IsSuccess);

        for (int i = 0; i < limit; i++)
        {
            Assert.True(reader.AcquirePet(Guid.NewGuid()).IsSuccess);
        }

        var result = reader.AcquirePet(Guid.NewGuid());

        Assert.False(result.IsSuccess);
        Assert.Equal(ReaderErrors.PetLimitReached.Code, result.Error.Code);
        Assert.Equal(limit, reader.GetPets().Count);
    }

    [Fact]
    public void AcquirePet_WhenPetAlreadyOwned_FailsWithPetAlreadyOwned()
    {
        var reader = ReaderFactory.CreateReader();
        var petId = Guid.NewGuid();
        reader.AcquirePet(petId);

        var result = reader.AcquirePet(petId);

        Assert.False(result.IsSuccess);
        Assert.Equal(ReaderErrors.PetAlreadyOwned.Code, result.Error.Code);
        Assert.Single(reader.GetPets());
    }

    [Fact]
    public void AcquirePet_WhenSucceeds_ReaderHasPet()
    {
        var reader = ReaderFactory.CreateReader();
        var petId = Guid.NewGuid();

        var result = reader.AcquirePet(petId);

        Assert.True(result.IsSuccess);
        Assert.True(reader.HasPet(petId));
        Assert.False(reader.HasPet(Guid.NewGuid()));
    }

    [Fact]
    public void AcquireBook_WhenSucceeds_ReturnsProgressIdAndRaisesBookAcquiredEvent()
    {
        var reader = ReaderFactory.CreateReader();
        var bookId = Guid.NewGuid();

        var result = reader.AcquireBook(bookId);

        Assert.True(result.IsSuccess);
        Assert.Equal([bookId], reader.GetBooks());
        Assert.Equal([result.Value], reader.GetProgresses());

        var acquired = Assert.IsType<BookAcquiredEvent>(Assert.Single(reader.PopDomainEvents()));
        Assert.Equal(reader.Id, acquired.ReaderId);
        Assert.Equal(bookId, acquired.BookId);
        Assert.Equal(result.Value, acquired.ProgressId);
    }

    [Fact]
    public void AcquireBook_WhenBookAlreadyOwned_FailsAndDoesNotRaiseAnotherEvent()
    {
        var reader = ReaderFactory.CreateReader();
        var bookId = Guid.NewGuid();
        reader.AcquireBook(bookId);
        reader.PopDomainEvents();

        var result = reader.AcquireBook(bookId);

        Assert.False(result.IsSuccess);
        Assert.Equal(ReaderErrors.BookAlreadyOwned.Code, result.Error.Code);
        Assert.Single(reader.GetBooks());
        Assert.Single(reader.GetProgresses());
        Assert.Empty(reader.PopDomainEvents());
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
        Assert.Equal(AccountType.Pro, reader.AccountType);
    }

    [Fact]
    public void ChangeAccountType_WhenPetsEqualNewLimit_Succeeds()
    {
        var reader = ReaderFactory.CreateProReader();

        for (int i = 0; i < 5; i++)
        {
            reader.AcquirePet(Guid.NewGuid());
        }

        var result = reader.ChangeAccountType(AccountType.Free);

        Assert.True(result.IsSuccess);
        Assert.Equal(AccountType.Free, reader.AccountType);
    }

    [Fact]
    public void AssignRole_WhenAssignedTwice_KeepsASingleRole()
    {
        var reader = ReaderFactory.CreateReader();

        reader.AssignRole(Role.Admin);
        reader.AssignRole(Role.Admin);

        Assert.Equal([Role.Admin], reader.GetRoles());
    }

    [Theory]
    [InlineData("Bob@Smith.com", "bob@smith.com")]
    [InlineData("  bob@smith.com  ", "bob@smith.com")]
    [InlineData("BOB@SMITH.COM", "bob@smith.com")]
    public void NormalizeEmail_TrimsAndLowercases(string email, string expected)
    {
        Assert.Equal(expected, Reader.NormalizeEmail(email));
    }
}
