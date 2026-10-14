using BookyPets.Application.Readers.Queries.GetReaderBooks;
using BookyPets.Application.Tests.Common;
using BookyPets.Domain.BookAggregate;
using BookyPets.Domain.ReaderAggregate;
using BookyPets.Shared.Mediator.Abstractions;
using BookyPets.Shared.Result;
using Common.Tests.Readers;
using Common.Tests.TestConstants;

namespace BookyPets.Application.Tests.Readers.Commands;

[Collection(MediatorFactoryCollection.CollectionName)]
public class AcquireBookTests(MediatorFactory mediatorFactory)
{
    private readonly TestScenario _scenario = mediatorFactory.CreateScenario();

    private IMediator Mediator => _scenario.Mediator;

    [Fact]
    public async Task AcquireBook_WhenValidCommand_ShouldAcquireBookAndCreateProgress()
    {
        await _scenario.RegisterReaderAsync();
        var bookId = await _scenario.CreateBookAsync();

        var acquireBookResult = await Mediator.SendAsync(ReaderCommandFactory.CreateAcquireBookCommand(bookId));

        Assert.True(acquireBookResult.IsSuccess);

        var progress = await _scenario.GetProgressAsync(acquireBookResult.Value);
        Assert.Equal(bookId, progress.BookId);
        Assert.Equal(0, progress.CurrentPage);
        Assert.Equal(Constants.Book.PageCount, progress.TotalPages);
        Assert.Equal(BookStatus.PlanToRead, progress.Status);

        var booksResult = await Mediator.SendAsync(new GetReaderBooksQuery());
        Assert.True(booksResult.IsSuccess);
        Assert.Equal(bookId, Assert.Single(booksResult.Value).Id);
    }

    [Fact]
    public async Task AcquireBook_WhenBookDoesNotExist_ShouldReturnNotFound()
    {
        await _scenario.RegisterReaderAsync();

        var acquireBookResult = await Mediator.SendAsync(ReaderCommandFactory.CreateAcquireBookCommand());

        Assert.False(acquireBookResult.IsSuccess);
        Assert.Equal(ErrorType.NotFound, acquireBookResult.Error.Type);
        Assert.Equal("BookNotFound", acquireBookResult.Error.Code);
    }

    [Fact]
    public async Task AcquireBook_WhenBookAlreadyOwned_ShouldReturnConflict()
    {
        await _scenario.RegisterReaderAsync();
        var bookId = await _scenario.CreateBookAsync();
        await _scenario.AcquireBookAsync(bookId);

        var result = await Mediator.SendAsync(ReaderCommandFactory.CreateAcquireBookCommand(bookId));

        Assert.False(result.IsSuccess);
        Assert.Equal(ErrorType.Conflict, result.Error.Type);
        Assert.Equal(ReaderErrors.BookAlreadyOwned.Code, result.Error.Code);
    }

    [Fact]
    public async Task AcquireBook_WhenTwoReadersAcquireTheSameBook_ShouldCreateASeparateProgressForEach()
    {
        await _scenario.RegisterReaderAsync();
        var bookId = await _scenario.CreateBookAsync();
        var firstProgressId = await _scenario.AcquireBookAsync(bookId);

        await _scenario.RegisterReaderAsync(Constants.Reader.OtherEmail);
        var secondProgressId = await _scenario.AcquireBookAsync(bookId);

        Assert.NotEqual(firstProgressId, secondProgressId);

        var firstProgressResult = await Mediator.SendAsync(ReaderCommandFactory.CreateGetProgressQuery(firstProgressId));
        Assert.False(firstProgressResult.IsSuccess);
        Assert.Equal(ProgressErrors.NotOwned.Code, firstProgressResult.Error.Code);
    }
}
