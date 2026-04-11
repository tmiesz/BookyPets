using ContractsGenre = BookyPets.Contracts.Books.Genre;
using DomainGenre = BookyPets.Domain.BookAggregate.Genre;

namespace BookyPets.Domain.Tests.TestConstants;

public static partial class Constants
{
    public static class Pet
    {
        public static readonly Guid Id = Guid.NewGuid();
        public static readonly string Name = "David";
        public static readonly DomainGenre FavouriteGenre = DomainGenre.Educational;
        public static readonly ContractsGenre FavouriteContractsGenre = ContractsGenre.Educational;
    }
}
