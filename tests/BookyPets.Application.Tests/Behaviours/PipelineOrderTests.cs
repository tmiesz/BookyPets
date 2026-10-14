using BookyPets.Application.Tests.Common;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;
using Common.Tests.Books;
using Common.Tests.Readers;
using Common.Tests.Sessions;
using Common.Tests.TestConstants;

namespace BookyPets.Application.Tests.Behaviours;

[Collection(MediatorFactoryCollection.CollectionName)]
public class PipelineOrderTests(MediatorFactory mediatorFactory)
{
    private readonly IMediator _mediator = mediatorFactory.CreateMediator();

    [Fact]
    public async Task SendAsync_WhenCallerIsNotAuthorizedAndRequestIsInvalid_ShouldReturnForbiddenInsteadOfValidationError()
    {
        mediatorFactory.FakeCurrentReaderProvider.SetCurrentReader(Guid.NewGuid(), Constants.Reader.Permissions, []);
        var invalidCommand = BookCommandFactory.CreateCreateBookCommand(title: "");

        var result = await _mediator.SendAsync(invalidCommand);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Forbidden, result.Error.Type);
        Assert.Equal("ReaderForbidden", result.Error.Code);
    }

    [Fact]
    public async Task SendAsync_WhenCallerIsAuthorizedAndRequestIsInvalid_ShouldReturnValidationError()
    {
        var invalidCommand = BookCommandFactory.CreateCreateBookCommand(title: "");

        var result = await _mediator.SendAsync(invalidCommand);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Validation, result.Error.Type);
        Assert.Equal("Title", result.Error.Code);
    }

    [Fact]
    public async Task SendAsync_WhenReaderLacksThePermission_ShouldReturnForbidden()
    {
        mediatorFactory.FakeCurrentReaderProvider.SetCurrentReader(Guid.NewGuid(), [], []);

        var result = await _mediator.SendAsync(SessionCommandFactory.CreateStartSessionCommand(Guid.NewGuid()));

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Forbidden, result.Error.Type);
        Assert.Equal("ReaderForbidden", result.Error.Code);
    }

    [Fact]
    public async Task SendAsync_WhenReaderIsNotAnAdmin_ShouldNotBeAbleToCreateBooks()
    {
        mediatorFactory.FakeCurrentReaderProvider.SetCurrentReader(Guid.NewGuid(), Constants.Reader.Permissions, []);

        var result = await _mediator.SendAsync(BookCommandFactory.CreateCreateBookCommand());

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Forbidden, result.Error.Type);
    }

    [Fact]
    public async Task SendAsync_WhenReaderLacksAcquirePermission_ShouldNotBeAbleToAcquireBooks()
    {
        mediatorFactory.FakeCurrentReaderProvider.SetCurrentReader(Guid.NewGuid(), [], []);

        var result = await _mediator.SendAsync(ReaderCommandFactory.CreateAcquireBookCommand(Guid.NewGuid()));

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Forbidden, result.Error.Type);
    }
}
