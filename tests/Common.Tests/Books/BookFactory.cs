using BookyPets.Domain.BookAggregate;
using BookyPets.Domain.Tests.TestConstants;

namespace Common.Tests.Books;

public static class BookFactory
{
    public static Book CreateBook(
        string? title = null,
        string? author = null,
        Genre? genre = null,
        int? pageCount = null,
        Guid? id = null)
    {
        return new Book(
           title: title ?? Constants.Book.Title,
           author: author ?? Constants.Book.Author,
           genre: genre ?? Constants.Book.Genre,
           pageCount: pageCount ?? Constants.Book.PageCount,
           id: id ?? Constants.Book.Id
        );
    }
}
