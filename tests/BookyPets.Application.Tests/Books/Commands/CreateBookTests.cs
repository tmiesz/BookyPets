using BookyPets.Application.Tests.Common;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;
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
    [InlineData(3)]
    [InlineData(100)]
    public async Task CreateBook_WhenTitleLengthIsAtTheBoundary_ShouldCreateBook(int bookTitleLength)
    {
        string bookTitle = new('a', bookTitleLength);
        var createBookCommand = BookCommandFactory.CreateCreateBookCommand(title: bookTitle);

        var result = await _mediator.SendAsync(createBookCommand);

        Assert.True(result.IsSuccess);
        Assert.Equal(bookTitle, result.Value.Title);
    }

    [Theory]
    [InlineData(0, "Value length must be at least 3")]
    [InlineData(1, "Value length must be at least 3")]
    [InlineData(2, "Value length must be at least 3")]
    [InlineData(101, "Value length must be at most 100")]
    [InlineData(200, "Value length must be at most 100")]
    public async Task CreateBook_WhenTitleLengthIsInvalid_ShouldReturnValidationError(int bookTitleLength, string expectedDescription)
    {
        string bookTitle = new('a', bookTitleLength);
        var createBookCommand = BookCommandFactory.CreateCreateBookCommand(title: bookTitle);

        var result = await _mediator.SendAsync(createBookCommand);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Validation, result.Error.Type);
        Assert.Equal("Title", result.Error.Code);
        Assert.Equal(expectedDescription, result.Error.Description);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task CreateBook_WhenPageCountIsNotPositive_ShouldReturnValidationError(int pageCount)
    {
        var createBookCommand = BookCommandFactory.CreateCreateBookCommand(pageCount: pageCount);

        var result = await _mediator.SendAsync(createBookCommand);

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Validation, result.Error.Type);
        Assert.Equal("PageCount", result.Error.Code);
    }

    [Fact]
    public async Task CreateBook_WhenSeveralFieldsAreInvalid_ShouldReturnOnlyTheFirstFailure()
    {
        var createBookCommand = BookCommandFactory.CreateCreateBookCommand(title: "", author: "");

        var result = await _mediator.SendAsync(createBookCommand);

        Assert.False(result.IsSuccess);
        Assert.Equal("Title", result.Error.Code);
    }
}
