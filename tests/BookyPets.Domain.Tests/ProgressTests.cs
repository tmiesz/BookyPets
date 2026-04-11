using BookyPets.Domain.BookAggregate;
using BookyPets.Domain.Tests.TestConstants;
using Common.Tests.Books;

namespace BookyPets.Domain.Tests;

public class ProgressTests
{
    [Fact]
    public void UpdateProgress_WhenExactPages_ShouldComplete()
    {
        var progress = ProgressFactory.CreateProgress();

        var result1 = progress.AddPagesRead(15);

        Assert.True(result1.IsSuccess);
        Assert.Equal(15, progress.CurrentPage);

        var result2 = progress.AddPagesRead(30);

        Assert.True(result2.IsSuccess);
        Assert.Equal(30, progress.CurrentPage);
    }

    [Fact]
    public void Status_WhenCurrentPageIsTotalPage_ShouldBeCompleted()
    {
        var book = BookFactory.CreateBook();

        var progress = ProgressFactory.CreateProgress(Guid.NewGuid(), book);

        var result = progress.AddPagesRead(Constants.Book.PageCount);

        Assert.True(result.IsSuccess);
        Assert.Equal(Constants.Book.PageCount, progress.CurrentPage);
        Assert.Equal(BookStatus.Completed, progress.Status);
    }
}
