using DomainBookStatus = BookyPets.Domain.BookAggregate.BookStatus;
using ContractsGenre = BookyPets.Contracts.Books.Genre;
using DomainGenre = BookyPets.Domain.BookAggregate.Genre;

namespace BookyPets.Domain.Tests.TestConstants;

public static partial class Constants
{

    public static class Progress
    {
        public static readonly Guid Id = Guid.NewGuid();
        public static readonly int CurrentPage = 0;
        public static readonly DomainBookStatus Status  = DomainBookStatus.Reading;
    }

    public static class Book
    {
        public static readonly Guid Id = Guid.NewGuid();
        public static readonly string Title = "The Software Engineer's Guidebook";
        public static readonly string Author = "Gergely Orosz";
        public static readonly DomainGenre Genre = DomainGenre.Educational;
        public static readonly ContractsGenre ContractsGenre = ContractsGenre.Educational;
        public static readonly int PageCount = 100;
    }
}
