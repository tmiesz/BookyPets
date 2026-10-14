using BookyPets.Domain.BookAggregate;
using BookyPets.Domain.SessionAggregate;
using Common.Tests.TestConstants;
using Common.Tests.TestUtils;

namespace Common.Tests.Sessions;

public static class SessionFactory
{
    public static Session CreateSession(
        Guid? readerId = null,
        Guid? progressId = null,
        Guid? bookId = null,
        Genre? genre = null,
        DateTime? startTime = null,
        Guid? petId = null)
    {
        return new Session(
            readerId: readerId ?? Constants.Reader.Id,
            progressId: progressId ?? Constants.Progress.Id,
            bookId: bookId ?? Constants.Book.Id,
            genre: genre ?? Constants.Book.Genre,
            startTime: startTime ?? new FakeDateTimeProvider().UtcNow,
            petId: petId ?? Constants.Pet.Id
        );
    }
}
