using BookyPets.Application.Books.Commands;
using BookyPets.Domain.BookAggregate;
using BookyPets.Domain.Tests.TestConstants;

namespace Common.Tests.Books;

public static class BookCommandFactory
{
    public static CreateBookCommand CreateCreateBookCommand(
        string? title = null,
        string? author = null,
        Genre? genre = null,
        int? pageCount = null)
    {
        return new CreateBookCommand(
            Title: title ?? Constants.Book.Title,
            Author: author ?? Constants.Book.Author,
            Genre: genre ?? Constants.Book.Genre,
            PageCount: pageCount ?? Constants.Book.PageCount);
    }
}
