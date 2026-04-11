using BookyPets.Application.Tests.Common;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;
using Common.Tests.Pets;
using Common.Tests.Readers;

namespace BookyPets.Application.Tests.Readers.Commands;

[Collection(MediatorFactoryCollection.CollectionName)]
public class AcquirePetTests(MediatorFactory mediatorFactory)
{
    private readonly IMediator _mediator = mediatorFactory.CreateMediator();

    [Fact]
    public async Task AcquirePet_WhenValidCommand_ShouldAcquirePet()
    {
        var registerCommand = ReaderCommandFactory.CreateRegisterCommand();
        var registerResult = await _mediator.SendAsync(registerCommand);
        Assert.True(registerResult.IsSuccess);

        mediatorFactory.FakeCurrentReaderProvider.SetCurrentReader(registerResult.Value.Reader.Id);

        var createPetCommand = PetCommandFactory.CreateCreatePetCommand();
        var createPetResult = await _mediator.SendAsync(createPetCommand);
        Assert.True(createPetResult.IsSuccess);

        var acquirePetCommand = ReaderCommandFactory.CreateAcquirePetCommand(petId: createPetResult.Value.Id);
        var acquirePetResult = await _mediator.SendAsync(acquirePetCommand);
        Assert.True(acquirePetResult.IsSuccess);
    }

    [Fact]
    public async Task AcquirePet_WhenPetDoesNotExist_ShouldReturnNotFound()
    {
        var registerCommand = ReaderCommandFactory.CreateRegisterCommand();
        var registerResult = await _mediator.SendAsync(registerCommand);
        Assert.True(registerResult.IsSuccess);

        mediatorFactory.FakeCurrentReaderProvider.SetCurrentReader(registerResult.Value.Reader.Id);

        var acquirePetCommand = ReaderCommandFactory.CreateAcquirePetCommand();
        var acquirePetResult = await _mediator.SendAsync(acquirePetCommand);
        Assert.False(acquirePetResult.IsSuccess);
        Assert.Equal(ErrorType.NotFound, acquirePetResult.Error.Type);
    }
}
