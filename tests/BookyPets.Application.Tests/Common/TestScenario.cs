using BookyPets.Application.Pets.Queries.GetPet;
using BookyPets.Application.Sessions.Common;
using BookyPets.Application.Sessions.Queries.GetActiveSession;
using BookyPets.Domain.BookAggregate;
using BookyPets.Domain.PetAggregate;
using BookyPets.Domain.SessionAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;
using Common.Tests.Books;
using Common.Tests.Pets;
using Common.Tests.Readers;
using Common.Tests.Sessions;
using Common.Tests.TestUtils;

namespace BookyPets.Application.Tests.Common;

public class TestScenario(IMediator mediator, FakeCurrentReaderProvider currentReader)
{
    public IMediator Mediator { get; } = mediator;

    public async Task<Guid> RegisterReaderAsync(string? email = null)
    {
        var result = await Mediator.SendAsync(ReaderCommandFactory.CreateRegisterCommand(email: email));
        AssertSucceeded(result);

        var readerId = result.Value.Reader.Id;
        currentReader.SetCurrentReader(readerId);

        return readerId;
    }

    public void SwitchTo(Guid readerId) => currentReader.SetCurrentReader(readerId);

    public async Task<Guid> CreateBookAsync(string? title = null, int? pageCount = null, Genre? genre = null)
    {
        var result = await Mediator.SendAsync(
            BookCommandFactory.CreateCreateBookCommand(title: title, pageCount: pageCount, genre: genre));
        AssertSucceeded(result);

        return result.Value.Id;
    }

    public async Task<Guid> AcquireBookAsync(Guid? bookId = null)
    {
        bookId ??= await CreateBookAsync();

        var result = await Mediator.SendAsync(ReaderCommandFactory.CreateAcquireBookCommand(bookId));
        AssertSucceeded(result);

        return result.Value;
    }

    public async Task<Guid> CreatePetAsync(string? name = null, Genre? genre = null)
    {
        var result = await Mediator.SendAsync(PetCommandFactory.CreateCreatePetCommand(name: name, genre: genre));
        AssertSucceeded(result);

        return result.Value.Id;
    }

    public async Task<Guid> AcquirePetAsync(Guid? petId = null)
    {
        petId ??= await CreatePetAsync();

        var result = await Mediator.SendAsync(ReaderCommandFactory.CreateAcquirePetCommand(petId));
        AssertSucceeded(result);

        return petId.Value;
    }

    public async Task<Session> StartSessionAsync(Guid progressId, Guid? petId = null)
    {
        var result = await Mediator.SendAsync(SessionCommandFactory.CreateStartSessionCommand(progressId, petId));
        AssertSucceeded(result);

        return result.Value;
    }

    public async Task<Progress> GetProgressAsync(Guid progressId)
    {
        var result = await Mediator.SendAsync(ReaderCommandFactory.CreateGetProgressQuery(progressId));
        AssertSucceeded(result);

        return result.Value;
    }

    public async Task<Pet> GetPetAsync(Guid petId)
    {
        var result = await Mediator.SendAsync(new GetPetQuery(petId));
        AssertSucceeded(result);

        return result.Value;
    }

    public async Task<ActiveSessionResult?> GetActiveSessionAsync()
    {
        var result = await Mediator.SendAsync(new GetActiveSessionQuery());
        AssertSucceeded(result);

        return result.Value;
    }

    private static void AssertSucceeded(Result result)
    {
        Assert.True(result.IsSuccess, result.IsSuccess ? null : $"Arrange step failed: {result.Error.Code} - {result.Error.Description}");
    }
}
