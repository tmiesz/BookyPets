using BookyPets.Application.Readers.Queries.GetReaderPets;
using BookyPets.Application.Tests.Common;
using BookyPets.Domain.ReaderAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;
using Common.Tests.Readers;

namespace BookyPets.Application.Tests.Readers.Commands;

[Collection(MediatorFactoryCollection.CollectionName)]
public class AcquirePetTests(MediatorFactory mediatorFactory)
{
    private readonly TestScenario _scenario = mediatorFactory.CreateScenario();

    private IMediator Mediator => _scenario.Mediator;

    [Fact]
    public async Task AcquirePet_WhenValidCommand_ShouldAcquirePet()
    {
        await _scenario.RegisterReaderAsync();
        var petId = await _scenario.CreatePetAsync();

        var acquirePetResult = await Mediator.SendAsync(ReaderCommandFactory.CreateAcquirePetCommand(petId));

        Assert.True(acquirePetResult.IsSuccess);

        var petsResult = await Mediator.SendAsync(new GetReaderPetsQuery());
        Assert.True(petsResult.IsSuccess);
        Assert.Equal(petId, Assert.Single(petsResult.Value).Id);
    }

    [Fact]
    public async Task AcquirePet_WhenPetDoesNotExist_ShouldReturnNotFound()
    {
        await _scenario.RegisterReaderAsync();

        var acquirePetResult = await Mediator.SendAsync(ReaderCommandFactory.CreateAcquirePetCommand());

        Assert.False(acquirePetResult.IsSuccess);
        Assert.Equal(ErrorType.NotFound, acquirePetResult.Error.Type);
        Assert.Equal("PetNotFound", acquirePetResult.Error.Code);
    }

    [Fact]
    public async Task AcquirePet_WhenPetAlreadyOwned_ShouldReturnConflict()
    {
        await _scenario.RegisterReaderAsync();
        var petId = await _scenario.CreatePetAsync();
        await _scenario.AcquirePetAsync(petId);

        var result = await Mediator.SendAsync(ReaderCommandFactory.CreateAcquirePetCommand(petId));

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Conflict, result.Error.Type);
        Assert.Equal(ReaderErrors.PetAlreadyOwned.Code, result.Error.Code);
    }

    [Fact]
    public async Task AcquirePet_WhenFreeAccountPetLimitIsReached_ShouldReturnPetLimitReached()
    {
        await _scenario.RegisterReaderAsync();

        for (int i = 0; i < 5; i++)
        {
            var petId = await _scenario.CreatePetAsync(name: $"Pet{i}");
            await _scenario.AcquirePetAsync(petId);
        }

        var sixthPetId = await _scenario.CreatePetAsync(name: "Pet5");

        var result = await Mediator.SendAsync(ReaderCommandFactory.CreateAcquirePetCommand(sixthPetId));

        Assert.False(result.IsSuccess);
        Assert.Equal(ReaderErrors.PetLimitReached.Code, result.Error.Code);

        var petsResult = await Mediator.SendAsync(new GetReaderPetsQuery());
        Assert.Equal(5, petsResult.Value.Count);
    }
}
