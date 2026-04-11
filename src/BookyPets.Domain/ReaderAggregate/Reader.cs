using BookyPets.Domain.BookAggregate.Events;
using BookyPets.Domain.Common;
using BookyPets.Domain.Common.Interfaces;
using BookyPets.Shared.Result;

namespace BookyPets.Domain.ReaderAggregate;

public class Reader : AggregateRoot
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    private readonly string _passwordHash = null!;
    public AccountType AccountType { get; private set; }
    private readonly List<string> _roles = [];

    private readonly List<Guid> _petIds = [];
    private readonly List<Guid> _bookIds = [];
    private readonly List<Guid> _progressIds = [];
    // private readonly List<Guid> _questIds = [];

    public bool HasPet(Guid id) => _petIds.Contains(id);

    private Reader()
    {
        FirstName = null!;
        LastName = null!;
        Email = null!;
        AccountType = null!;
    }

    public Reader(string firstName, string lastName, string email, string passwordHash, Guid? id = null) : base(id ?? Guid.NewGuid())
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        _passwordHash = passwordHash;
        AccountType = AccountType.Free;
    }

    public bool IsCorrectPasswordHash(string password, IPasswordHasher passwordHasher)
    {
        return passwordHasher.IsCorrectPassword(password, _passwordHash);
    }

    public void AssignRole(string role)
    {
        if (!_roles.Contains(role))
            _roles.Add(role);
    }

    public List<string> GetRoles() => _roles.ToList();

    public Result AcquirePet(Guid petId)
    {
        if (_petIds.Count >= AccountType.MaxOwnedPets())
            return ReaderErrors.PetLimitReached;

        if (_petIds.Contains(petId))
            return ReaderErrors.PetAlreadyOwned;

        _petIds.Add(petId);

        return Result.Success;
    }

    public Result<Guid> AcquireBook(Guid bookId)
    {
        if (_bookIds.Contains(bookId))
            return ReaderErrors.BookAlreadyOwned;

        var progressId = Guid.NewGuid();

        _bookIds.Add(bookId);
        _progressIds.Add(progressId);
        _domainEvents.Add(new BookAcquiredEvent(Id, bookId, progressId));

        return progressId;
    }

    // public Result AcquireQuest(Guid questId)
    // {
    //     if (_questIds.Count >= AccountType.MaxActiveQuests())
    //         return ReaderErrors.QuestLimitReached;

    //     if (_questIds.Contains(questId))
    //         return ReaderErrors.QuestAlreadyActive;

    //     _questIds.Add(questId);
    //     return Result.Success;
    // }

    public Result ChangeAccountType(AccountType newAccountType)
    {
        if (newAccountType.MaxOwnedPets() < _petIds.Count)
            return ReaderErrors.TooManyPetsForDowngrade;

        // if (newAccountType.MaxActiveQuests() < _questIds.Count)
        //     return ReaderErrors.TooManyQuestsForDowngrade;

        AccountType = newAccountType;

        return Result.Success;
    }
}
