using BookyPets.Domain.BookAggregate;
using BookyPets.Domain.Tests.TestConstants;

namespace Common.Tests.Books;

public static class ProgressFactory
{
    public static Progress CreateProgress(
        Guid? readerId = null,
        Book? book = null,
        Guid? id = null)
    {
        return new Progress(
           readerId: readerId ?? Constants.Reader.Id,
           book: book ?? BookFactory.CreateBook(),
           id: id ?? Constants.Progress.Id
        );
    }
}
