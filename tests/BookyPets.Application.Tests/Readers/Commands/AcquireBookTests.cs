using BookyPets.Application.Tests.Common;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;
using Common.Tests.Books;
using Common.Tests.Readers;

namespace BookyPets.Application.Tests.Readers.Commands;

[Collection(MediatorFactoryCollection.CollectionName)]
public class AcquireBookTests(MediatorFactory mediatorFactory)
{
    private readonly IMediator _mediator = mediatorFactory.CreateMediator();

    [Fact]
    public async Task AcquireBook_WhenValidCommand_ShouldAcquireBook()
    {
        var registerCommand = ReaderCommandFactory.CreateRegisterCommand();
        var registerResult = await _mediator.SendAsync(registerCommand);
        Assert.True(registerResult.IsSuccess);

        mediatorFactory.FakeCurrentReaderProvider.SetCurrentReader(registerResult.Value.Reader.Id);

        var createBookCommand = BookCommandFactory.CreateCreateBookCommand();
        var createBookResult = await _mediator.SendAsync(createBookCommand);
        Assert.True(createBookResult.IsSuccess);

        var acquireBookCommand = ReaderCommandFactory.CreateAcquireBookCommand(bookId: createBookResult.Value.Id);
        var acquireBookResult = await _mediator.SendAsync(acquireBookCommand);
        Assert.True(acquireBookResult.IsSuccess);
    }

    [Fact]
    public async Task AcquireBook_WhenBookDoesNotExist_ShouldReturnNotFound()
    {
        var registerCommand = ReaderCommandFactory.CreateRegisterCommand();
        var registerResult = await _mediator.SendAsync(registerCommand);
        Assert.True(registerResult.IsSuccess);

        mediatorFactory.FakeCurrentReaderProvider.SetCurrentReader(registerResult.Value.Reader.Id);
        var acquireBookCommand = ReaderCommandFactory.CreateAcquireBookCommand();
        var acquireBookResult = await _mediator.SendAsync(acquireBookCommand);
        Assert.False(acquireBookResult.IsSuccess);
        Assert.Equal(ErrorType.NotFound, acquireBookResult.Error.Type);
    }
}
