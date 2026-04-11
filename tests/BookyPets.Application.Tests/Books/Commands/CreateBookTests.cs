using BookyPets.Application.Tests.Common;
using BookyPets.Shared.Mediator.Abstractions;
using Common.Tests.Books;

namespace BookyPets.Application.Tests.Books.Commands;

[Collection(MediatorFactoryCollection.CollectionName)]
public class CreateBookTests(MediatorFactory mediatorFactory)
{
    private readonly IMediator _mediator = mediatorFactory.CreateMediator();

    [Fact]
    public async Task CreateBook_WhenValidCommand_ShouldCreateBook()
    {
        var createBookCommand = BookCommandFactory.CreateCreateBookCommand();

        var result = await _mediator.SendAsync(createBookCommand);

        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(200)]
    public async Task CreateBook_WhenCommandContainsInvalidData_ShouldReturnValidationError(int bookTitleLength)
    {
        string bookTitle = new('a', bookTitleLength);
        var createBookCommand = BookCommandFactory.CreateCreateBookCommand(title: bookTitle);

        var result = await _mediator.SendAsync(createBookCommand);

        Assert.False(result.IsSuccess);
        Assert.Equal("Title", result.Error.Code);
    }
}
